using MeuProjetoApi.Models;
using NUlid;

namespace MeuProjetoApi.Models
{
    public class Instituicao
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();
        public string Nome { get; set; } = string.Empty;
        public string? CNPJ { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

        public ICollection<Escolaridade> EscolaridadesAssociadas { get; set; } = new List<Escolaridade>();
    }

    
}

