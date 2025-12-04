using System;

namespace BibliotecaPublica.Domain.Models;

public class Exemplar
{
    public Guid Id { get; set; } = Guid.NewGuid(); // id único do exemplar
    public Guid LivroId { get; set; }
    public Livro? Livro { get; set; }

    public Guid? FilialId { get; set; }
    public Filial? Filial { get; set; }

    public string? CodigoBarras { get; set; }
    public bool Disponivel { get; set; } = true;
    public string? Condicao { get; set; } // ex.: Novo, Bom, Gasto, Danificado
}
