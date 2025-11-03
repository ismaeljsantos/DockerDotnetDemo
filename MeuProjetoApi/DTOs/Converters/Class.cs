using MeuProjetoApi.DTOs.Converters; // Necessário
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
// ... outros usings

namespace MeuProjetoApi.DTOs
{
    public class PessoaCriacaoDtodfr
    {
        // ... (Nome, CPF, etc.)

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        // APLICAÇÃO DO CONVERSOR CUSTOMIZADO
        [JsonConverter(typeof(JsonDataNascimentoConverter))]
        public DateTime DataNascimento { get; set; }

        // ...
    }
}