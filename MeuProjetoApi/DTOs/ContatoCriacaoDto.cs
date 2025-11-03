using System.ComponentModel.DataAnnotations;

namespace MeuProjetoApi.DTOs
{
    public class ContatoCriacaoDto
    {
        [Required(ErrorMessage = "O valor do contato (ex: número ou email) é obrigatório")]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "O valor do contato (ex: número ou email) é obrigatório")]
        [StringLength(100)]
        public string Valor { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Observacao { get; set; }

        public bool IsPrincipal { get; set; } = false;

    }
}
