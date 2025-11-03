using MeuProjetoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata.Builders; // Necessário para ConfigureConventions
using NUlid;
using System;
using MeuProjetoApi.Data.Converters;

namespace MeuProjetoApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // 1. Definição das Tabelas (DbSet)
        public DbSet<Pessoa> Pessoas { get; set; } = null!;
        public DbSet<Endereco> Enderecos { get; set; } = null!;
        public DbSet<Contato> Contatos { get; set; } = null!;
        public DbSet<Instituicao> Instituicoes { get; set; } = null!;
        public DbSet<Escolaridade> Escolaridades { get; set; } = null!;
        public DbSet<Empresa> Empresas { get; set; } = null!;
        public DbSet<ExperienciaProfissional> ExperienciasProfissionais { get; set; } = null!;
        public DbSet<Setor> Setores { get; set; } = null!;
        public DbSet<Funcao> Funcoes { get; set; } = null!;
        public DbSet<PessoaFuncao> PessoaFuncoes { get; set; } = null!;

        // 2. Mapeamento Global: Configurando ULID para GUID/uniqueidentifier
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {

            // Aplica a conversão para todas as propriedades do tipo Ulid (não anulável)
            configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToGuidConverter>() // <--- Chamada simplificada
            .HaveColumnType("uniqueidentifier");

            // Linhas 41-42: Aplica a conversão para Ulid? (anulável)
            configurationBuilder
            .Properties<Ulid?>()
            .HaveConversion<UlidToGuidConverter>() // <--- Chamada simplificada
            .HaveColumnType("uniqueidentifier");
       
        }


        // 3. Mapeamento Específico: Índices e Relacionamentos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Regras de Domínio ---

            // Pessoa: Índice Único para CpfHash
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.HasIndex(p => p.CpfHash).IsUnique();
            });

            // Endereco: Chave Estrangeira para Pessoa
            modelBuilder.Entity<Endereco>(entity =>
            {
                entity.HasOne(e => e.Pessoa)
                      .WithMany(p => p.Enderecos)
                      .HasForeignKey(e => e.FkPessoaId);
            });

            // Contato: Chave Estrangeira para Pessoa
            modelBuilder.Entity<Contato>(entity =>
            {
                entity.HasOne(c => c.Pessoa)
                      .WithMany(p => p.Contatos)
                      .HasForeignKey(c => c.FkPessoaId);
            });

            // Escolaridade: Chaves Estrangeiras para Pessoa e Instituicao
            modelBuilder.Entity<Escolaridade>(entity =>
            {
                entity.HasOne(esc => esc.Pessoa)
                      .WithMany(p => p.Escolaridades)
                      .HasForeignKey(esc => esc.FkPessoaId);

                entity.HasOne(esc => esc.Instituicao)
                      .WithMany(i => i.EscolaridadesAssociadas)
                      .HasForeignKey(esc => esc.FkInstituicaoId);
            });

            // Experiencia Profissional: Chaves Estrangeiras para Pessoa e Empresa
            modelBuilder.Entity<ExperienciaProfissional>(entity =>
            {
                entity.HasOne(exp => exp.Pessoa)
                      .WithMany(p => p.Experiencias)
                      .HasForeignKey(exp => exp.FkPessoaId);

                entity.HasOne(exp => exp.Empresa)
                      .WithMany(e => e.ExperienciasAssociadas)
                      .HasForeignKey(exp => exp.FkEmpresaId);
            });

            // Configuração da Tabela de Ligação PessoaFuncao (Muitos-para-Muitos)
            // Configuração da Tabela de Ligação PessoaFuncao
            modelBuilder.Entity<PessoaFuncao>(entity =>
            {
                // 1. Chave Primária Composta
                entity.HasKey(pf => new { pf.FkPessoaId, pf.FkFuncaoId, pf.FkSetorId });

                // 2. Relacionamento com Pessoa
                entity.HasOne(pf => pf.Pessoa)
                      .WithMany(p => p.PessoasFuncoes) // Adicione esta ICollection<PessoaFuncao> à classe Pessoa
                      .HasForeignKey(pf => pf.FkPessoaId);

                // 3. Relacionamento com Funcao
                entity.HasOne(pf => pf.Funcao)
                      .WithMany(f => f.PessoasFuncoes)
                      .HasForeignKey(pf => pf.FkFuncaoId);

                // 4. Relacionamento com Setor
                entity.HasOne(pf => pf.Setor)
                      .WithMany(s => s.PessoasFuncoes)
                      .HasForeignKey(pf => pf.FkSetorId);
            });
        }
    }
}