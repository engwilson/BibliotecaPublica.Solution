using System;

namespace BibliotecaPublica.Domain.Models;

public class Editora
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public string? Contato { get; set; }
}
