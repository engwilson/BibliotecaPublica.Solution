using System;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{


    public record ExemplarDto(Guid Id, Guid LivroId, Guid? FilialId, string? CodigoBarras, bool Disponivel, string? Condicao);

}