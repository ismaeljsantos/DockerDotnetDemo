using System.ComponentModel.DataAnnotations;
using System;

namespace MeuProjetoApi.DTOs
{
    public class ExperienciaCriacaoDto
    {
        [Required]
        public string NomeEmpresa { get; set; } = string.Empty;

        [Required]
        public string Funcao { get; set; } = string.Empty;

        public int? AnoEntrada { get; set; }
        public int? AnoSaida { get; set; }
    }
}