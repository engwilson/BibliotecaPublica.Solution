using System;
using System.Collections.Generic;

namespace BibliotecaPublica.Domain.Models;

public class Autor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PrimeiroNome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public string? Biografia { get; set; }

    // Navegação
    public List<Livro> Livros { get; set; } = new();

    public string NomeCompleto => $"{PrimeiroNome} {Sobrenome}";
}
