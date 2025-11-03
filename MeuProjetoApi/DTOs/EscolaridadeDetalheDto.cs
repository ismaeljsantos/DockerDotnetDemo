using NUlid;

namespace MeuProjetoApi.DTOs
{
    public class EscolaridadeDetalheDto
    {
        public Ulid Id { get; set; }
        public string NomeInstituicao { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string? NomeCurso { get; set; }
        public int? AnoInicio { get; set; }
        public int? AnoConclusao { get; set; }
        public bool Ativo { get; set; }

        public static EscolaridadeDetalheDto FromEntity(Escolaridade escolaridade)
        {
            return new EscolaridadeDetalheDto
            {
                Id = escolaridade.Id,
                NomeInstituicao = escolaridade.Instituicao?.Nome ?? "Instituição Desconhecida",
                Tipo = escolaridade.Tipo,
                NomeCurso = escolaridade.NomeCurso,
                AnoInicio = escolaridade.AnoInicio,
                AnoConclusao = escolaridade.AnoConclusao,
                Ativo = escolaridade.Ativo
            };
        }
    }
}
