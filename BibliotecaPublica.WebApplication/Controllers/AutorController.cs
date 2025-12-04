using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BibliotecaPublica.Service;
using BibliotecaPublica.Data.Context;
using BibliotecaPublica.Domain.Models;
using BibliotecaPublica.WebApplication.Controllers.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaPublica.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutorController : ControllerBase
{
    private readonly AutorService _service;
    private readonly BibliotecaPublicaDBContext _db;

    public AutorController(AutorService service, BibliotecaPublicaDBContext db)
    {
        _service = service;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var autores = await _service.ObterTodosAsync(cancellationToken);
        var dtos = autores.Select(a => new AutorDto(a.Id, a.NomeCompleto, a.Biografia)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var a = await _service.ObterPorIdAsync(id, cancellationToken);
        if (a is null) return NotFound();
        return Ok(new AutorDto(a.Id, a.NomeCompleto, a.Biografia));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AutorCreateDto dto, CancellationToken cancellationToken)
    {
        var autor = new Autor
        {
            PrimeiroNome = dto.PrimeiroNome,
            Sobrenome = dto.Sobrenome,
            Biografia = dto.Biografia
        };

        var criado = await _service.CriarAsync(autor, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, new AutorDto(criado.Id, criado.NomeCompleto, criado.Biografia));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] AutorUpdateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();

        var autor = new Autor
        {
            Id = dto.Id,
            PrimeiroNome = dto.PrimeiroNome,
            Sobrenome = dto.Sobrenome,
            Biografia = dto.Biografia
        };

        var ok = await _service.AtualizarAsync(autor, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _service.RemoverAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}
