using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;

namespace BibliotecaPublica.Service;

public class FilialService
{
    private readonly BibliotecaPublicaDBContext _db;

    public FilialService(BibliotecaPublicaDBContext db)
    {
        _db = db;
    }

    public async Task<Filial> CriarAsync(Filial filial, CancellationToken cancellationToken = default)
    {
        _db.Filiais.Add(filial);
        await _db.SaveChangesAsync(cancellationToken);
        return filial;
    }

    public async Task<IEnumerable<Filial>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Filiais
            .AsNoTracking()
            .Include(f => f.Exemplares)
            .ToListAsync(cancellationToken);
    }

    public async Task<Filial?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Filiais
            .AsNoTracking()
            .Include(f => f.Exemplares)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<bool> AtualizarAsync(Filial filial, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Filiais
            .Include(f => f.Exemplares)
            .FirstOrDefaultAsync(f => f.Id == filial.Id, cancellationToken);

        if (existente is null)
            return false;

        existente.Nome = filial.Nome;
        existente.Endereco = filial.Endereco;
        existente.Telefone = filial.Telefone;

        // Substitui exemplares se fornecido
        existente.Exemplares.Clear();
        if (filial.Exemplares is not null)
        {
            foreach (var ex in filial.Exemplares)
                existente.Exemplares.Add(ex);
        }

        _db.Filiais.Update(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Filiais.FindAsync(new object?[] { id }, cancellationToken);
        if (existente is null)
            return false;

        _db.Filiais.Remove(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
