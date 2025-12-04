using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Service;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;
using BibliotecaPublica.WebApplication.Controllers.DTOs;

namespace BibliotecaPublica.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LivroController : ControllerBase
{
    private readonly LivroService _service;
    private readonly BibliotecaPublicaDBContext _db;

    public LivroController(LivroService service, BibliotecaPublicaDBContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var livros = await _service.ObterTodosAsync(cancellationToken);
        var dtos = livros.Select(l => new LivroDto
        {
            Id = l.Id,
            Titulo = l.Titulo,
            Subtitulo = l.Subtitulo,
            Descricao = l.Descricao,
            ISBN = l.ISBN,
            DataPublicacao = l.DataPublicacao,
            EditoraId = l.EditoraId,
            EditoraNome = l.Editora?.Nome,
            AutorIds = l.Autores?.Select(a => a.Id).ToList() ?? new(),
            AutorNomes = l.Autores?.Select(a => a.NomeCompleto).ToList() ?? new(),
            TotalExemplares = l.TotalExemplares,
            ExemplaresDisponiveis = l.ExemplaresDisponiveis
        }).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var l = await _service.ObterPorIdAsync(id, cancellationToken);
        if (l is null) return NotFound();
        var dto = new LivroDto
        {
            Id = l.Id,
            Titulo = l.Titulo,
            Subtitulo = l.Subtitulo,
            Descricao = l.Descricao,
            ISBN = l.ISBN,
            DataPublicacao = l.DataPublicacao,
            EditoraId = l.EditoraId,
            EditoraNome = l.Editora?.Nome,
            AutorIds = l.Autores?.Select(a => a.Id).ToList() ?? new(),
            AutorNomes = l.Autores?.Select(a => a.NomeCompleto).ToList() ?? new(),
            TotalExemplares = l.TotalExemplares,
            ExemplaresDisponiveis = l.ExemplaresDisponiveis
        };
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] LivroCreateDto dto, CancellationToken cancellationToken)
    {
        var livro = new Livro
        {
            Titulo = dto.Titulo,
            Subtitulo = dto.Subtitulo,
            Descricao = dto.Descricao,
            ISBN = dto.ISBN,
            DataPublicacao = dto.DataPublicacao,
            EditoraId = dto.EditoraId,
            TotalExemplares = dto.TotalExemplares,
            ExemplaresDisponiveis = dto.ExemplaresDisponiveis
        };

        // resolve autores se enviado
        if (dto.AutorIds?.Any() == true)
        {
            var autores = await _db.Autores.Where(a => dto.AutorIds.Contains(a.Id)).ToListAsync(cancellationToken);
            foreach (var a in autores) livro.Autores.Add(a);
        }

        var criado = await _service.CriarAsync(livro, cancellationToken);
        var resultDto = new LivroDto
        {
            Id = criado.Id,
            Titulo = criado.Titulo,
            Subtitulo = criado.Subtitulo,
            Descricao = criado.Descricao,
            ISBN = criado.ISBN,
            DataPublicacao = criado.DataPublicacao,
            EditoraId = criado.EditoraId,
            EditoraNome = criado.Editora?.Nome,
            AutorIds = criado.Autores?.Select(a => a.Id).ToList() ?? new(),
            AutorNomes = criado.Autores?.Select(a => a.NomeCompleto).ToList() ?? new(),
            TotalExemplares = criado.TotalExemplares,
            ExemplaresDisponiveis = criado.ExemplaresDisponiveis
        };

        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, resultDto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] LivroUpdateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();

        var livro = new Livro
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Subtitulo = dto.Subtitulo,
            Descricao = dto.Descricao,
            ISBN = dto.ISBN,
            DataPublicacao = dto.DataPublicacao,
            EditoraId = dto.EditoraId,
            TotalExemplares = dto.TotalExemplares,
            ExemplaresDisponiveis = dto.ExemplaresDisponiveis
        };

        if (dto.AutorIds?.Any() == true)
        {
            var autores = await _db.Autores.Where(a => dto.AutorIds.Contains(a.Id)).ToListAsync(cancellationToken);
            foreach (var a in autores) livro.Autores.Add(a);
        }

        var ok = await _service.AtualizarAsync(livro, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _service.RemoverAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}
