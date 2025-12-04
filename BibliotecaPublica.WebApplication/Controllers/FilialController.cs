
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
public class FilialController : ControllerBase
{
    private readonly FilialService _service;

    public FilialController(FilialService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _service.ObterTodosAsync(cancellationToken);
        var dtos = list.Select(f => new BibliotecaPublica.WebApplication.Controllers.DTOs.FilialDto(f.Id, f.Nome, f.Endereco, f.Telefone)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var f = await _service.ObterPorIdAsync(id, cancellationToken);
        if (f is null) return NotFound();
        return Ok(new BibliotecaPublica.WebApplication.Controllers.DTOs.FilialDto(f.Id, f.Nome, f.Endereco, f.Telefone));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.FilialCreateDto dto, CancellationToken cancellationToken)
    {
        var filial = new Filial { Nome = dto.Nome, Endereco = dto.Endereco, Telefone = dto.Telefone };
        var criado = await _service.CriarAsync(filial, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, new BibliotecaPublica.WebApplication.Controllers.DTOs.FilialDto(criado.Id, criado.Nome, criado.Endereco, criado.Telefone));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.FilialUpdateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();
        var filial = new Filial { Id = dto.Id, Nome = dto.Nome, Endereco = dto.Endereco, Telefone = dto.Telefone };
        var ok = await _service.AtualizarAsync(filial, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _service.RemoverAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}