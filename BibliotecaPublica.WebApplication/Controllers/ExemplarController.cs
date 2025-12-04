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
public class ExemplarController : ControllerBase
{
    private readonly ExemplarService _service;

    public ExemplarController(ExemplarService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _service.ObterTodosAsync(cancellationToken);
        var dtos = list.Select(e => new BibliotecaPublica.WebApplication.Controllers.DTOs.ExemplarDto(e.Id, e.LivroId, e.FilialId ?? Guid.Empty, e.CodigoBarras, e.Disponivel, e.Condicao)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var e = await _service.ObterPorIdAsync(id, cancellationToken);
        if (e is null) return NotFound();
        return Ok(new BibliotecaPublica.WebApplication.Controllers.DTOs.ExemplarDto(e.Id, e.LivroId, e.FilialId ?? Guid.Empty, e.CodigoBarras, e.Disponivel, e.Condicao));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.ExemplarCreateDto dto, CancellationToken cancellationToken)
    {
        var exemplar = new Exemplar
        {
            LivroId = dto.LivroId,
            FilialId = dto.FilialId,
            CodigoBarras = dto.CodigoBarras,
            Disponivel = dto.Disponivel,
            Condicao = dto.Condicao
        };

        var criado = await _service.CriarAsync(exemplar, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, new BibliotecaPublica.WebApplication.Controllers.DTOs.ExemplarDto(criado.Id, criado.LivroId, criado.FilialId ?? Guid.Empty, criado.CodigoBarras, criado.Disponivel, criado.Condicao));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.ExemplarUpdateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();

        var exemplar = new Exemplar
        {
            Id = dto.Id,
            LivroId = dto.LivroId,
            FilialId = dto.FilialId,
            CodigoBarras = dto.CodigoBarras,
            Disponivel = dto.Disponivel,
            Condicao = dto.Condicao
        };

        var ok = await _service.AtualizarAsync(exemplar, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _service.RemoverAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}
