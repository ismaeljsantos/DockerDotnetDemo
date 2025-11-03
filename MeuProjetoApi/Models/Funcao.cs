using NUlid;

namespace MeuProjetoApi.Models
{
    public class Funcao
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();
        public string Titulo { get; set; } = string.Empty;

        public ICollection<PessoaFuncao> PessoasFuncoes { get; set; } = new List<PessoaFuncao>();
    }
}
