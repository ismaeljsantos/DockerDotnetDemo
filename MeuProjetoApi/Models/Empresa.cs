using MeuProjetoApi.Models;
using NUlid;

namespace MeuProjetoApi.Models
{
    public class Empresa
    {
        public Ulid Id { get; set; } = Ulid.NewUlid();
        public string Nome { get; set; } = string.Empty;
        public string? CNPJ { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

        public ICollection<ExperienciaProfissional> ExperienciasAssociadas { get; set; } = new List<ExperienciaProfissional>();
    }
}

public class ExperienciaProfissional
{
    public Ulid Id { get; set; } = Ulid.NewUlid();

    public Ulid FkPessoaId { get; set; }
    public Ulid FkEmpresaId { get; set; }

    public string Funcao { get; set; } = string.Empty;
    public int? AnoEntrada { get; set; }
    public int? AnoSaida { get; set; }
    public bool Ativo { get; set; } = true;

    public Pessoa Pessoa { get; set; } = null!;
    public Empresa Empresa { get; set; } = null!;
}