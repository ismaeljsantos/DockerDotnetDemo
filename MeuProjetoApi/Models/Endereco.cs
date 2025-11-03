
using NUlid;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeuProjetoApi.Models
{
    public class Endereco
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();

        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; } 
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;

        public bool IsPrincipal { get; set; } = false;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Pessoa))]
        public Ulid FkPessoaId { get; set; }
        public Pessoa Pessoa { get; set; } = null!;
    }
}
