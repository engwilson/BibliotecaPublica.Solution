using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{


    public record EditoraDto(Guid Id, string Nome, string? Endereco, string? Contato);
}
