using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BibliotecaPublica.Domain.Models;
using BibliotecaPublica.Domain.Enums;

namespace BibliotecaPublica.Data.Context;

public class BibliotecaPublicaDBContext : DbContext
{
    public BibliotecaPublicaDBContext(DbContextOptions<BibliotecaPublicaDBContext> options) : base(options)
    {
    }

    public DbSet<Livro> Livros { get; set; } = null!;
    public DbSet<Autor> Autores { get; set; } = null!;
    public DbSet<Editora> Editoras { get; set; } = null!;
    public DbSet<Filial> Filiais { get; set; } = null!;
    public DbSet<Exemplar> Exemplares { get; set; } = null!;
    public DbSet<Associado> Associados { get; set; } = null!;
    public DbSet<Emprestimo> Emprestimos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Livro
        modelBuilder.Entity<Livro>(b =>
        {
            b.ToTable("Livros");
            b.HasKey(x => x.Id);
            b.Property(x => x.Titulo).IsRequired().HasMaxLength(500);
            b.Property(x => x.ISBN).HasMaxLength(50);
            b.HasOne(x => x.Editora)
             .WithMany()
             .HasForeignKey(x => x.EditoraId)
             .OnDelete(DeleteBehavior.SetNull);

            // Many-to-many Livro <-> Autor (usando tabela de junção automática)
            b.HasMany(x => x.Autores)
             .WithMany(x => x.Livros)
             .UsingEntity<Dictionary<string, object>>(
                 "LivroAutor",
                 r => r.HasOne<Autor>().WithMany().HasForeignKey("AutorId").OnDelete(DeleteBehavior.Cascade),
                 l => l.HasOne<Livro>().WithMany().HasForeignKey("LivroId").OnDelete(DeleteBehavior.Cascade),
                 je =>
                 {
                     je.HasKey("LivroId", "AutorId");
                     je.ToTable("LivroAutores");
                 });

            // A propriedade Categorias é uma coleção de enum (List<CategoriaLivro>) — não mapeamos diretamente
            b.Ignore(x => x.Categorias);
        });

        // Autor
        modelBuilder.Entity<Autor>(b =>
        {
            b.ToTable("Autores");
            b.HasKey(x => x.Id);
            b.Property(x => x.PrimeiroNome).IsRequired().HasMaxLength(200);
            b.Property(x => x.Sobrenome).IsRequired().HasMaxLength(200);
        });

        // Editora
        modelBuilder.Entity<Editora>(b =>
        {
            b.ToTable("Editoras");
            b.HasKey(x => x.Id);
            b.Property(x => x.Nome).IsRequired().HasMaxLength(300);
        });

        // Filial
        modelBuilder.Entity<Filial>(b =>
        {
            b.ToTable("Filiais");
            b.HasKey(x => x.Id);
            b.Property(x => x.Nome).IsRequired().HasMaxLength(300);
        });

        // Exemplar
        modelBuilder.Entity<Exemplar>(b =>
        {
            b.ToTable("Exemplares");
            b.HasKey(x => x.Id);
            b.Property(x => x.CodigoBarras).HasMaxLength(100);
            b.HasOne(x => x.Livro)
             .WithMany()
             .HasForeignKey(x => x.LivroId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Filial)
             .WithMany(f => f.Exemplares)
             .HasForeignKey(x => x.FilialId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // Associado
        modelBuilder.Entity<Associado>(b =>
        {
            b.ToTable("Associados");
            b.HasKey(x => x.Id);
            b.Property(x => x.PrimeiroNome).IsRequired().HasMaxLength(200);
            b.Property(x => x.Sobrenome).IsRequired().HasMaxLength(200);
        });

        // Emprestimo
        modelBuilder.Entity<Emprestimo>(b =>
        {
            b.ToTable("Emprestimos");
            b.HasKey(x => x.Id);

            b.HasOne(x => x.Exemplar)
             .WithMany()
             .HasForeignKey(x => x.ExemplarId)
             .OnDelete(DeleteBehavior.SetNull);

            // Evita múltiplos caminhos em cascata no SQL Server
            b.HasOne(x => x.Livro)
             .WithMany()
             .HasForeignKey(x => x.LivroId)
             .OnDelete(DeleteBehavior.NoAction);

            b.HasOne(x => x.Associado)
             .WithMany(a => a.Emprestimos)
             .HasForeignKey(x => x.AssociadoId)
             .OnDelete(DeleteBehavior.Cascade);

            // Persiste enum como inteiro
            b.Property(x => x.Status).HasConversion<int>();
        });
    }
}
