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
public class AssociadoController : ControllerBase
{
    private readonly AssociadoService _service;

    public AssociadoController(AssociadoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _service.ObterTodosAsync(cancellationToken);
        var dtos = list.Select(a => new BibliotecaPublica.WebApplication.Controllers.DTOs. AssociadoDto(a.Id, a.NomeCompleto, a.Email, a.Telefone, a.DataAssociacao, a.Ativo)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var a = await _service.ObterPorIdAsync(id, cancellationToken);
        if (a is null) return NotFound();
        return Ok(new BibliotecaPublica.WebApplication.Controllers.DTOs.AssociadoDto(a.Id, a.NomeCompleto, a.Email, a.Telefone, a.DataAssociacao, a.Ativo));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.AssociadoCreateDto dto, CancellationToken cancellationToken)
    {
        var assoc = new Associado { PrimeiroNome = dto.PrimeiroNome, Sobrenome = dto.Sobrenome, Email = dto.Email, Telefone = dto.Telefone };
        var criado = await _service.CriarAsync(assoc, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, new BibliotecaPublica.WebApplication.Controllers.DTOs.AssociadoDto(criado.Id, criado.NomeCompleto, criado.Email, criado.Telefone, criado.DataAssociacao, criado.Ativo));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.AssociadoUpdateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();
        var assoc = new Associado { Id = dto.Id, PrimeiroNome = dto.PrimeiroNome, Sobrenome = dto.Sobrenome, Email = dto.Email, Telefone = dto.Telefone, Ativo = dto.Ativo, DataAssociacao = dto.DataAssociacao };
        var ok = await _service.AtualizarAsync(assoc, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _service.RemoverAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}
