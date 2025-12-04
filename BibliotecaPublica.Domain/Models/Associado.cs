using System;
using System.Collections.Generic;

namespace BibliotecaPublica.Domain.Models;

public class Associado
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PrimeiroNome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefone { get; set; }

    public DateTime DataAssociacao { get; set; } = DateTime.UtcNow;
    public bool Ativo { get; set; } = true;

    // navegação
    public List<Emprestimo> Emprestimos { get; set; } = new();

    public string NomeCompleto => $"{PrimeiroNome} {Sobrenome}";
}
