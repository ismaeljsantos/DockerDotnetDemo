namespace MeuProjetoApi.DTOs
{
    public class ExperienciaCriacaoDto
    {
        public string NomeEmpresa { get; set; } = string.Empty;
        public string Funcao { get; set; } = string.Empty;
        public int? AnoEntrada { get; set; }
        public int? AnoSaida { get; set; }
    }
}
