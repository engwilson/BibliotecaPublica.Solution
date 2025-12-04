using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{


    public record AutorDto(Guid Id, string NomeCompleto, string? Biografia);

}