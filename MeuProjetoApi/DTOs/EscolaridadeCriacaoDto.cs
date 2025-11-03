using System.ComponentModel.DataAnnotations;

namespace MeuProjetoApi.DTOs
{
    public class EscolaridadeCriacaoDto
    {
        [Required]
        public string NomeInstituicao { get; set; } = string.Empty;
        [Required]
        public string Tipo { get; set; } = string.Empty;
        public string? NomeCurso { get; set; } 

        public int? AnoInicio { get; set; }
        public int? AnoConclusao { get; set; }
    }
}
