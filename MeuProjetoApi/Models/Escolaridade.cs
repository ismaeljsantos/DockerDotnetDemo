using MeuProjetoApi.Models;
using NUlid;

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