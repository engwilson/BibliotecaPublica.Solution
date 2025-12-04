using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;

namespace BibliotecaPublica.Service;

public class AssociadoService
{
    private readonly BibliotecaPublicaDBContext _db;

    public AssociadoService(BibliotecaPublicaDBContext db)
    {
        _db = db;
    }

    public async Task<Associado> CriarAsync(Associado associado, CancellationToken cancellationToken = default)
    {
        _db.Associados.Add(associado);
        await _db.SaveChangesAsync(cancellationToken);
        return associado;
    }

    public async Task<IEnumerable<Associado>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Associados
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Associado?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Associados
            .AsNoTracking()
            .Include(a => a.Emprestimos)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> AtualizarAsync(Associado associado, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Associados.FirstOrDefaultAsync(a => a.Id == associado.Id, cancellationToken);
        if (existente is null)
            return false;

        existente.PrimeiroNome = associado.PrimeiroNome;
        existente.Sobrenome = associado.Sobrenome;
        existente.Email = associado.Email;
        existente.Telefone = associado.Telefone;
        existente.Ativo = associado.Ativo;
        existente.DataAssociacao = associado.DataAssociacao;

        _db.Associados.Update(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Associados.FindAsync(new object?[] { id }, cancellationToken);
        if (existente is null)
            return false;

        _db.Associados.Remove(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
