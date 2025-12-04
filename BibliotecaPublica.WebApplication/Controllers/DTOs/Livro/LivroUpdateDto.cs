using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class LivroUpdateDto : LivroCreateDto
    {
        public Guid Id { get; set; }
    }
}
