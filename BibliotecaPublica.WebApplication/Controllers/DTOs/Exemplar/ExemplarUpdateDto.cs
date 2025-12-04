using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class ExemplarUpdateDto : ExemplarCreateDto
    {
        public Guid Id { get; set; }
    }
}
