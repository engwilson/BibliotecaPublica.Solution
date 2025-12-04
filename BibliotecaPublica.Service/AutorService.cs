using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;

namespace BibliotecaPublica.Service;

public class AutorService
{
    private readonly BibliotecaPublicaDBContext _db;

    public AutorService(BibliotecaPublicaDBContext db)
    {
        _db = db;
    }

    public async Task<Autor> CriarAsync(Autor autor, CancellationToken cancellationToken = default)
    {
        _db.Autores.Add(autor);
        await _db.SaveChangesAsync(cancellationToken);
        return autor;
    }

    public async Task<IEnumerable<Autor>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Autores
            .AsNoTracking()
            .Include(a => a.Livros)
            .ToListAsync(cancellationToken);
    }

    public async Task<Autor?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Autores
            .AsNoTracking()
            .Include(a => a.Livros)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> AtualizarAsync(Autor autor, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Autores
            .Include(a => a.Livros)
            .FirstOrDefaultAsync(a => a.Id == autor.Id, cancellationToken);

        if (existente is null)
            return false;

        existente.PrimeiroNome = autor.PrimeiroNome;
        existente.Sobrenome = autor.Sobrenome;
        existente.Biografia = autor.Biografia;

        // Substitui associação de livros (se fornecida)
        existente.Livros.Clear();
        if (autor.Livros is not null)
        {
            foreach (var livro in autor.Livros)
                existente.Livros.Add(livro);
        }

        _db.Autores.Update(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Autores.FindAsync(new object?[] { id }, cancellationToken);
        if (existente is null)
            return false;

        _db.Autores.Remove(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
