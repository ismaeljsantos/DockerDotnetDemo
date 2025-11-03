
using NUlid;
using System.Data;

namespace MeuProjetoApi.Models
{
    public class Pessoa
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string CpfCriptografado { get; set; } = string.Empty;
        public string CpfHash { get; set; } = string.Empty;
        public string NomeMae { get; set; } = string.Empty;
        public string NomePai { get; set; } = string.Empty;

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public ICollection<Contato> Contatos { get; set; } = new List<Contato>();
        public ICollection<Escolaridade> Escolaridades { get; set; } = new List<Escolaridade>();
        public ICollection<ExperienciaProfissional> Experiencias { get; set; } = new List<ExperienciaProfissional>();
        public ICollection<PessoaFuncao> PessoasFuncoes { get; set; } = new List<PessoaFuncao>();
    }
}