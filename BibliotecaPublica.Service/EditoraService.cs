using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;

namespace BibliotecaPublica.Service;

public class EditoraService
{
    private readonly BibliotecaPublicaDBContext _db;

    public EditoraService(BibliotecaPublicaDBContext db)
    {
        _db = db;
    }

    public async Task<Editora> CriarAsync(Editora editora, CancellationToken cancellationToken = default)
    {
        _db.Editoras.Add(editora);
        await _db.SaveChangesAsync(cancellationToken);
        return editora;
    }

    public async Task<IEnumerable<Editora>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Editoras
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Editora?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Editoras
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> AtualizarAsync(Editora editora, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Editoras.FirstOrDefaultAsync(e => e.Id == editora.Id, cancellationToken);
        if (existente is null)
            return false;

        existente.Nome = editora.Nome;
        existente.Endereco = editora.Endereco;
        existente.Contato = editora.Contato;

        _db.Editoras.Update(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Editoras.FindAsync(new object?[] { id }, cancellationToken);
        if (existente is null)
            return false;

        _db.Editoras.Remove(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
