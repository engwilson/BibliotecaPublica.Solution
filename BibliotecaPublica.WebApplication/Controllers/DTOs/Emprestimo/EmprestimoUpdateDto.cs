using System;
using BibliotecaPublica.Domain.Enums;
namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class EmprestimoUpdateDto : EmprestimoCreateDto
    {
        public Guid Id { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public StatusEmprestimo Status { get; set; }
    }
}
