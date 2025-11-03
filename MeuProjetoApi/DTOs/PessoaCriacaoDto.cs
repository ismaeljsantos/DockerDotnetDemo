using MeuProjetoApi.DTOs.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace MeuProjetoApi.DTOs
{
    public class PessoaCriacaoDto
    {
        private string _cpf = string.Empty;

        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [JsonConverter(typeof(JsonDataNascimentoConverter))]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter  11 digitos.")]
        public string Cpf
        {
            get => _cpf;
            set
            {
                // Remove qualquer caractere que não seja dígito
                _cpf = Regex.Replace(value, @"[^\d]", string.Empty); 
            }
        }

        [Required(ErrorMessage = "O campo Nome da mãe é obrigatório.")]
        public string NomeMae { get; set; } = string.Empty;
        public string? NomePai { get; set; } 

        public ICollection<EnderecoCriacaoDto>? Enderecos { get; set; }
        public ICollection<ContatoCriacaoDto>? Contatos { get; set; }
        public ICollection<EscolaridadeCriacaoDto>? Escolaridades { get; set; }
        public ICollection<ExperienciaCriacaoDto>? Experiencias { get; set; }
    }
}
