using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{


    public class AutorUpdateDto : AutorCreateDto
    {
        public Guid Id { get; set; }
    }
}
