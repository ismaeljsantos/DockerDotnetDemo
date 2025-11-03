using NUlid;

namespace MeuProjetoApi.Models
{
    public class PessoaFuncao
    {
        public Ulid FkPessoaId { get; set; }
        public Ulid FkFuncaoId { get; set; }
        public Ulid FkSetorId { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public Pessoa Pessoa { get; set; } = null!;
        public Funcao Funcao { get; set; } = null!;
        public Setor Setor { get; set; } = null!;
    }
}
