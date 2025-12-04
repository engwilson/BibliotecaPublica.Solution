using System;
using System.Collections.Generic;

namespace BibliotecaPublica.Domain.Models;

public class Filial
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string? Endereco { get; set; }
    public string? Telefone { get; set; }

    // exemplares localizados nesta filial
    public List<Exemplar> Exemplares { get; set; } = new();
}
