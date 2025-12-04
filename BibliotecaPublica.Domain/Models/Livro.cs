using System;
using System.Collections.Generic;
using BibliotecaPublica.Domain.Enums;

namespace BibliotecaPublica.Domain.Models;

public class Livro
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Titulo { get; set; } = string.Empty;
    public string? Subtitulo { get; set; }
    public string? Descricao { get; set; }
    public string? ISBN { get; set; }
    public DateTime? DataPublicacao { get; set; }

    // Relacionamentos
    public Guid? EditoraId { get; set; }
    public Editora? Editora { get; set; }

    public List<Autor> Autores { get; set; } = new();
    public List<CategoriaLivro> Categorias { get; set; } = new();

    // Resumo de inventário
    public int TotalExemplares { get; set; } = 0;
    public int ExemplaresDisponiveis { get; set; } = 0;
}
