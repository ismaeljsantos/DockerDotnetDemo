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

public class Escolaridade
{
    public Ulid Id { get; set; } = Ulid.NewUlid();

    public Ulid FkPessoaId { get; set; }
    public Ulid FkInstituicaoId { get; set; }

    public string Tipo { get; set; } = string.Empty;
    public string? NomeCurso { get; set; }

    public int? AnoInicio { get; set; }
    public int? AnoConclusao { get; set; }

    public bool Ativo { get; set; } = true;

    public Pessoa Pessoa { get; set; } = null!;
    public Instituicao Instituicao { get; set; } = null!;

}