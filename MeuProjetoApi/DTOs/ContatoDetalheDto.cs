using MeuProjetoApi.Models;
using NUlid;

namespace MeuProjetoApi.DTOs
{
    public class ContatoDetalheDto
    {
        public Ulid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Valor { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public bool IsPrincipal { get; set; } = false;

        public static ContatoDetalheDto FromEntity(Contato contato)
        {
            return new ContatoDetalheDto
            {
                Id = contato.Id,
                Tipo = contato.Tipo,
                Valor = contato.Valor,
                Observacao = contato.Observacao,
                IsPrincipal = contato.IsPrincipal
            };
        }
    }
}
