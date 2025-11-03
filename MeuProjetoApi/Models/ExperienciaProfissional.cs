using NUlid;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeuProjetoApi.Models
{
    public class ExperienciaProfissional
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();

        public string Funcao { get; set; } = string.Empty;
        public int? AnoEntrada { get; set; }
        public int? AnoSaida { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Pessoa))]
        public Ulid FkPessoaId { get; set; }
        public Pessoa Pessoa { get; set; } = null!;

        [ForeignKey(nameof(Empresa))]
        public Ulid FkEmpresaId { get; set; }
        public Empresa Empresa { get; set; } = null!;
    }
}
