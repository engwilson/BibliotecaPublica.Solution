using System;
using System.Collections.Generic;

namespace BibliotecaPublica.WebApplication.Controllers.DTOs
{
    public class LivroCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? ISBN { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public Guid? EditoraId { get; set; }
        public List<Guid>? AutorIds { get; set; }
        public int TotalExemplares { get; set; }
        public int ExemplaresDisponiveis { get; set; }
    }
}
