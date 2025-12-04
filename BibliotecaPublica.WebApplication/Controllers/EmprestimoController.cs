using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BibliotecaPublica.Service;
using BibliotecaPublica.Domain.Models;
using BibliotecaPublica.Domain.Enums;

namespace BibliotecaPublica.WebApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmprestimoController : ControllerBase
{
    private readonly EmprestimoService _service;

    public EmprestimoController(EmprestimoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var list = await _service.ObterTodosAsync(cancellationToken);
        var dtos = list.Select(e => new BibliotecaPublica.WebApplication.Controllers.DTOs.EmprestimoDto(e.Id, e.ExemplarId ?? Guid.Empty, e.LivroId ?? Guid.Empty, e.AssociadoId, e.DataEmprestimo, e.DataVencimento, e.DataDevolucao, (int)e.Status)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var e = await _service.ObterPorIdAsync(id, cancellationToken);
        if (e is null) return NotFound();
        return Ok(new BibliotecaPublica.WebApplication.Controllers.DTOs.EmprestimoDto(e.Id, e.ExemplarId ?? Guid.Empty, e.LivroId ?? Guid.Empty, e.AssociadoId, e.DataEmprestimo, e.DataVencimento, e.DataDevolucao, (int)e.Status));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.EmprestimoCreateDto dto, CancellationToken cancellationToken)
    {
        var emprestimo = new Emprestimo
        {
            ExemplarId = dto.ExemplarId,
            LivroId = dto.LivroId,
            AssociadoId = dto.AssociadoId,
            DataVencimento = dto.DataVencimento
        };

        var criado = await _service.CriarAsync(emprestimo, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, new BibliotecaPublica.WebApplication.Controllers.DTOs.EmprestimoDto(criado.Id, criado.ExemplarId ?? Guid.Empty, criado.LivroId ?? Guid.Empty, criado.AssociadoId, criado.DataEmprestimo, criado.DataVencimento, criado.DataDevolucao, (int)criado.Status));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] BibliotecaPublica.WebApplication.Controllers.DTOs.EmprestimoUpdateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id) return BadRequest();

        var emprestimo = new Emprestimo
        {
            Id = dto.Id,
            ExemplarId = dto.ExemplarId,
            LivroId = dto.LivroId,
            AssociadoId = dto.AssociadoId,
            DataVencimento = dto.DataVencimento,
            DataDevolucao = dto.DataDevolucao,
            Status = dto.Status
        };

        var ok = await _service.AtualizarAsync(emprestimo, cancellationToken);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var ok = await _service.RemoverAsync(id, cancellationToken);
        return ok ? NoContent() : NotFound();
    }
}
