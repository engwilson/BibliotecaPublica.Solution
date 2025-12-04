using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class AssociadoUpdateDto : AssociadoCreateDto
    {
        public Guid Id { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAssociacao { get; set; }
    }
}
