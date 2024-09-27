using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace CRUDAPI.models;

public partial class RfscamContext : DbContext
{
    public RfscamContext()
    {
    }

    public RfscamContext(DbContextOptions<RfscamContext> options): base(options)
    {
    }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Empregado> Empregados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.DeptoId).HasName("PRIMARY");

            entity.ToTable("departamento", tb => tb.HasComment("Criação da tabela \"Departamento\""));

            entity.HasIndex(e => e.Nome, "nome_UNIQUE").IsUnique();

            entity.Property(e => e.DeptoId).HasColumnName("depto_id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Empregado>(entity =>
        {
            entity.HasKey(e => e.EmprId).HasName("PRIMARY");

            entity.ToTable("empregado", tb => tb.HasComment("Criação da tabela \"Empregado\""));

            entity.HasIndex(e => e.EmprId, "empr_id_UNIQUE").IsUnique();

            entity.Property(e => e.EmprId).HasColumnName("empr_id");
            entity.Property(e => e.Cargo)
                .HasMaxLength(100)
                .HasColumnName("cargo");
            entity.Property(e => e.DeptoId)
                .HasMaxLength(100)
                .HasColumnName("depto_id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .HasColumnName("nome");

            entity.HasMany(d => d.Deptos).WithMany(p => p.Emprs)
                .UsingEntity<Dictionary<string, object>>(
                    "EmprPossuiDepto",
                    r => r.HasOne<Departamento>().WithMany()
                        .HasForeignKey("DeptoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_empr_has_depto_dep1"),
                    l => l.HasOne<Empregado>().WithMany()
                        .HasForeignKey("EmprId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_empr_possui_depto_empr"),
                    j =>
                    {
                        j.HasKey("EmprId", "DeptoId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("empr_possui_depto", tb => tb.HasComment("Criação da tabela de relacionamento entre as tabelas \"Empregado\" e \"Departamento\""));
                        j.HasIndex(new[] { "DeptoId" }, "fk_empr_possui_depto_dep1_idx");
                        j.HasIndex(new[] { "EmprId" }, "fk_empr_possui_depto_empr_idx");
                        j.IndexerProperty<int>("EmprId").HasColumnName("empr_id");
                        j.IndexerProperty<int>("DeptoId").HasColumnName("depto_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}