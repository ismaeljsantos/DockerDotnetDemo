using NUlid;

namespace MeuProjetoApi.Models
{
    public class Setor
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();
        public string Nome { get; set; } = string.Empty;

        public ICollection<PessoaFuncao> PessoasFuncoes { get; set; } = new List<PessoaFuncao>();
    }
}
