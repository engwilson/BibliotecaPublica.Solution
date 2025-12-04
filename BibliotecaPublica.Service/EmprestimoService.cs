using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;
using BibliotecaPublica.Domain.Enums;

namespace BibliotecaPublica.Service;

public class EmprestimoService
{
    private readonly BibliotecaPublicaDBContext _db;

    public EmprestimoService(BibliotecaPublicaDBContext db)
    {
        _db = db;
    }

    public async Task<Emprestimo> CriarAsync(Emprestimo emprestimo, CancellationToken cancellationToken = default)
    {
        // Define status ativo ao criar
        emprestimo.Status = StatusEmprestimo.Ativo;
        emprestimo.DataEmprestimo = DateTime.UtcNow;

        if (emprestimo.ExemplarId is not null)
        {
            var exemplar = await _db.Exemplares.Include(e => e.Livro).FirstOrDefaultAsync(e => e.Id == emprestimo.ExemplarId, cancellationToken);
            if (exemplar is not null)
            {
                exemplar.Disponivel = false;
                if (exemplar.Livro is not null)
                {
                    exemplar.Livro.ExemplaresDisponiveis = Math.Max(0, exemplar.Livro.ExemplaresDisponiveis - 1);
                }
            }
        }

        _db.Emprestimos.Add(emprestimo);
        await _db.SaveChangesAsync(cancellationToken);
        return emprestimo;
    }

    public async Task<IEnumerable<Emprestimo>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Emprestimos
            .AsNoTracking()
            .Include(e => e.Exemplar)
            .Include(e => e.Livro)
            .Include(e => e.Associado)
            .ToListAsync(cancellationToken);
    }

    public async Task<Emprestimo?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Emprestimos
            .AsNoTracking()
            .Include(e => e.Exemplar)
            .Include(e => e.Livro)
            .Include(e => e.Associado)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> AtualizarAsync(Emprestimo emprestimo, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Emprestimos
            .Include(e => e.Exemplar)
            .ThenInclude(x => x!.Livro)
            .FirstOrDefaultAsync(e => e.Id == emprestimo.Id, cancellationToken);

        if (existente is null)
            return false;

        // Detecta devolução: se status mudou para Devolvido e não havia data de devolução
        var antesStatus = existente.Status;
        existente.DataVencimento = emprestimo.DataVencimento;
        existente.DataDevolucao = emprestimo.DataDevolucao;
        existente.Status = emprestimo.Status;

        if (antesStatus != emprestimo.Status && emprestimo.Status == StatusEmprestimo.Devolvido)
        {
            if (existente.DataDevolucao is null)
                existente.DataDevolucao = DateTime.UtcNow;

            if (existente.Exemplar is not null)
            {
                existente.Exemplar.Disponivel = true;
                if (existente.Exemplar.Livro is not null)
                    existente.Exemplar.Livro.ExemplaresDisponiveis++;
            }
        }

        _db.Emprestimos.Update(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Emprestimos
            .Include(e => e.Exemplar)
            .ThenInclude(x => x!.Livro)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (existente is null)
            return false;

        // Ao remover empréstimo ativo, tenta devolver o exemplar logicamente
        if (existente.Status == StatusEmprestimo.Ativo && existente.Exemplar is not null)
        {
            existente.Exemplar.Disponivel = true;
            if (existente.Exemplar.Livro is not null)
                existente.Exemplar.Livro.ExemplaresDisponiveis++;
        }

        _db.Emprestimos.Remove(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
