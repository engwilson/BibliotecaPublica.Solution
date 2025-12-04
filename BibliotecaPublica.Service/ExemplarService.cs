using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;

namespace BibliotecaPublica.Service;

public class ExemplarService
{
    private readonly BibliotecaPublicaDBContext _db;

    public ExemplarService(BibliotecaPublicaDBContext db)
    {
        _db = db;
    }

    public async Task<Exemplar> CriarAsync(Exemplar exemplar, CancellationToken cancellationToken = default)
    {
        // Atualiza contagem no livro se existir
        if (exemplar.LivroId != Guid.Empty)
        {
            var livro = await _db.Livros.FirstOrDefaultAsync(l => l.Id == exemplar.LivroId, cancellationToken);
            if (livro is not null)
            {
                livro.TotalExemplares++;
                if (exemplar.Disponivel)
                    livro.ExemplaresDisponiveis++;
            }
        }

        _db.Exemplares.Add(exemplar);
        await _db.SaveChangesAsync(cancellationToken);
        return exemplar;
    }

    public async Task<IEnumerable<Exemplar>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Exemplares
            .AsNoTracking()
            .Include(e => e.Livro)
            .Include(e => e.Filial)
            .ToListAsync(cancellationToken);
    }

    public async Task<Exemplar?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Exemplares
            .AsNoTracking()
            .Include(e => e.Livro)
            .Include(e => e.Filial)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> AtualizarAsync(Exemplar exemplar, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Exemplares
            .Include(e => e.Livro)
            .FirstOrDefaultAsync(e => e.Id == exemplar.Id, cancellationToken);

        if (existente is null)
            return false;

        // Ajusta contagem de disponibilidade se necessário
        if (exemplar.Disponivel != existente.Disponivel && existente.Livro is not null)
        {
            if (exemplar.Disponivel)
                existente.Livro.ExemplaresDisponiveis++;
            else
                existente.Livro.ExemplaresDisponiveis--;
        }

        existente.CodigoBarras = exemplar.CodigoBarras;
        existente.Condicao = exemplar.Condicao;
        existente.Disponivel = exemplar.Disponivel;
        existente.FilialId = exemplar.FilialId;
        existente.LivroId = exemplar.LivroId;

        _db.Exemplares.Update(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Exemplares
            .Include(e => e.Livro)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (existente is null)
            return false;

        // Ajusta contagem no livro
        if (existente.Livro is not null)
        {
            existente.Livro.TotalExemplares = Math.Max(0, existente.Livro.TotalExemplares - 1);
            if (existente.Disponivel)
                existente.Livro.ExemplaresDisponiveis = Math.Max(0, existente.Livro.ExemplaresDisponiveis - 1);
        }

        _db.Exemplares.Remove(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
