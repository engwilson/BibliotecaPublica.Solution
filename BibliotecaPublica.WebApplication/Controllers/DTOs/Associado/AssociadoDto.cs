using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{


    public record AssociadoDto(Guid Id, string NomeCompleto, string? Email, string? Telefone, DateTime DataAssociacao, bool Ativo);

}