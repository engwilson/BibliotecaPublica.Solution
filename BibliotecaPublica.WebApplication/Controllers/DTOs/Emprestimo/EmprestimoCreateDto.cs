using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class EmprestimoCreateDto
    {
        public Guid? ExemplarId { get; set; }
        public Guid? LivroId { get; set; }
        public Guid AssociadoId { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
