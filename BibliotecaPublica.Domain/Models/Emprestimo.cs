using System;
using BibliotecaPublica.Domain.Enums;

namespace BibliotecaPublica.Domain.Models;

public class Emprestimo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // se rastrear por exemplar:
    public Guid? ExemplarId { get; set; }
    public Exemplar? Exemplar { get; set; }

    // conveniência: link para livro
    public Guid? LivroId { get; set; }
    public Livro? Livro { get; set; }

    public Guid AssociadoId { get; set; }
    public Associado? Associado { get; set; }

    public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;
    public DateTime DataVencimento { get; set; }
    public DateTime? DataDevolucao { get; set; }

    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Solicitado;

    public bool EstaAtrasado => Status == StatusEmprestimo.Ativo && DateTime.UtcNow > DataVencimento;
}
