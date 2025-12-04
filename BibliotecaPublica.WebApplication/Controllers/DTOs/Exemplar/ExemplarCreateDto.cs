using System;
namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class ExemplarCreateDto
    {
        public Guid LivroId { get; set; }
        public Guid? FilialId { get; set; }
        public string? CodigoBarras { get; set; }
        public bool Disponivel { get; set; } = true;
        public string? Condicao { get; set; }
    }
}
