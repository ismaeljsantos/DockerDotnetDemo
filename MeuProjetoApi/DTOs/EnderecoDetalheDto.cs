using MeuProjetoApi.Models;
using NUlid;

namespace MeuProjetoApi.DTOs
{
    public class EnderecoDetalheDto
    {
        public Ulid Id { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public bool IsPrincipal { get; set; } = false;

        public static EnderecoDetalheDto FromEntity(Endereco endereco)
        {
            return new EnderecoDetalheDto
            {
                Id = endereco.Id,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Cep = endereco.Cep,
                IsPrincipal = endereco.IsPrincipal
            };
        }
    }
}
