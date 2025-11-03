using NUlid;
using System.ComponentModel.DataAnnotations.Schema;
// Adicionar System para DateTime
using System;

namespace MeuProjetoApi.Models
{
    public class ExperienciaProfissional
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();
        public string Funcao { get; set; } = string.Empty;
        public int? AnoEntrada { get; set; } // Propriedade de Modelo
        public int? AnoSaida { get; set; }   // Propriedade de Modelo
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Pessoa))]
        public Ulid FkPessoaId { get; set; }
        public Pessoa Pessoa { get; set; } = null!; // Navegação

        [ForeignKey(nameof(Empresa))]
        public Ulid FkEmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!; // Navegação
    }
}