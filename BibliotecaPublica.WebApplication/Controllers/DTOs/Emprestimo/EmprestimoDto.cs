using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{


    public record EmprestimoDto(Guid Id, Guid? ExemplarId, Guid? LivroId, Guid AssociadoId, DateTime DataEmprestimo, DateTime DataVencimento, DateTime? DataDevolucao, int Status);

}