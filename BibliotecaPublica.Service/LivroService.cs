using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;

namespace BibliotecaPublica.Service;

public class LivroService
{
    private readonly BibliotecaPublicaDBContext _db;

    public LivroService(BibliotecaPublicaDBContext db)
    {
        _db = db;
    }

    public async Task<Livro> CriarAsync(Livro livro, CancellationToken cancellationToken = default)
    {
        _db.Livros.Add(livro);
        await _db.SaveChangesAsync(cancellationToken);
        return livro;
    }

    public async Task<IEnumerable<Livro>> ObterTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Livros
            .AsNoTracking()
            .Include(l => l.Autores)
            .Include(l => l.Editora)
            .ToListAsync(cancellationToken);
    }

    public async Task<Livro?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Livros
            .AsNoTracking()
            .Include(l => l.Autores)
            .Include(l => l.Editora)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<bool> AtualizarAsync(Livro livro, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Livros
            .Include(l => l.Autores)
            .FirstOrDefaultAsync(l => l.Id == livro.Id, cancellationToken);

        if (existente is null)
            return false;

        // Atualiza propriedades escalares
        existente.Titulo = livro.Titulo;
        existente.Subtitulo = livro.Subtitulo;
        existente.Descricao = livro.Descricao;
        existente.ISBN = livro.ISBN;
        existente.DataPublicacao = livro.DataPublicacao;
        existente.EditoraId = livro.EditoraId;
        existente.TotalExemplares = livro.TotalExemplares;
        existente.ExemplaresDisponiveis = livro.ExemplaresDisponiveis;

        // Atualiza autores (substitui a lista)
        existente.Autores.Clear();
        if (livro.Autores is not null)
        {
            foreach (var autor in livro.Autores)
                existente.Autores.Add(autor);
        }

        _db.Livros.Update(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var existente = await _db.Livros.FindAsync(new object?[] { id }, cancellationToken);
        if (existente is null)
            return false;

        _db.Livros.Remove(existente);
        await _db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
