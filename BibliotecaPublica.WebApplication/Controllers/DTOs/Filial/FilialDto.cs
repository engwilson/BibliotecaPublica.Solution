using System;
namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{


    public record FilialDto(Guid Id, string Nome, string? Endereco, string? Telefone);

}