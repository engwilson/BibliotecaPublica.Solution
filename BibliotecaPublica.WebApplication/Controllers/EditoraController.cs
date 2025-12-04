using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BibliotecaPublica.Service;
using BibliotecaPublica.Domain.Models;

namespace BibliotecaPublica.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EditoraController : ControllerBase
{
    private readonly EditoraService _service;

    public EditoraController(EditoraService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _service.ObterTodosAsync(cancellationToken);
        var dtos = list.Select(e => new BibliotecaPublica.WebApplication.Controllers.DTOs.EditoraDto(e.Id, e.Nome, e.Endereco, e.Contato)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var e = await _service.ObterPorIdAsync(id, cancellationToken);
        if (e is null) return NotFound();
        return Ok(new BibliotecaPublica.WebApplication.Controllers.DTOs.EditoraDto(e.Id, e.Nome, e.Endereco, e.Contato));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.EditoraCreateDto dto, CancellationToken cancellationToken)
    {
        var editora = new Editora { Nome = dto.Nome, Endereco = dto.Endereco, Contato = dto.Contato };
        var criado = await _service.CriarAsync(editora, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, new BibliotecaPublica.WebApplication.Controllers.DTOs.EditoraDto(criado.Id, criado.Nome, criado.Endereco, criado.Contato));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.EditoraUpdateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();
        var editora = new Editora { Id = dto.Id, Nome = dto.Nome, Endereco = dto.Endereco, Contato = dto.Contato };
        var ok = await _service.AtualizarAsync(editora, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _service.RemoverAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}
