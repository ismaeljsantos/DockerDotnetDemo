
using System.ComponentModel.DataAnnotations;

namespace MeuProjetoApi.DTOs
{
    public class EnderecoCriacaoDto
    {
        [Required(ErrorMessage = "O logradouro é obrigatorio.")]
        [StringLength(150)]
        public string Logradouro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O numero é obrigatorio.")]
        [StringLength(10)]
        public string Numero { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatorio.")]
        [StringLength(100)]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O cidade é obrigatorio.")]
        [StringLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Estado (UF) é obrigatorio.")]
        [StringLength(2)]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CEP é obrigatorio.")]
        public string Cep { get; set; } = string.Empty;

        public bool IsPrincipal { get; set; } = false;
    }
}
