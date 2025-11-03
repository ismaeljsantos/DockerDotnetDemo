using NUlid;

namespace MeuProjetoApi.DTOs
{
    public class ExperienciaDetalheDto
    {
        public Ulid Id { get; set; }
        public string NomeEmpresa { get; set; } = string.Empty;
        public string Funcao { get; set; } = string.Empty;
        public int? AnoEntrada { get; set; }
        public int? AnoSaida { get; set; }

        public static ExperienciaDetalheDto FromEntity(ExperienciaProfissional experiencia)
        {
            return new ExperienciaDetalheDto
            {
                Id = experiencia.Id,
                NomeEmpresa = experiencia.Empresa?.Nome ?? "Empresa Desconhecida",
                Funcao = experiencia.Funcao,
                AnoEntrada = experiencia.AnoEntrada,
                AnoSaida = experiencia.AnoSaida
            };
        }
    }
}
