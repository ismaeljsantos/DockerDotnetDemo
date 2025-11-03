

using NUlid;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeuProjetoApi.Models
{
    public class Contato
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();

        public string Tipo { get; set; } = "Telefone";

        public string Valor { get; set; } = string.Empty;
        public string? Observacao { get; set; }

        public bool IsPrincipal { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Pessoa))]
        public Ulid FkPessoaId { get; set; }
        public Pessoa Pessoa { get; set; } = null!;
    }
}
