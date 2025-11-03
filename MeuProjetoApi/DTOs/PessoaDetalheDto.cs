using System;
using System.Collections.Generic;
using System.Linq; 
using MeuProjetoApi.Models;
using NUlid;

namespace MeuProjetoApi.DTOs
{
    public class PessoaDetalheDto
    {
        public Ulid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string NomeMae { get; set; } = string.Empty;
        public string? NomePai { get; set; } = string.Empty;

        public ICollection<EnderecoDetalheDto> Enderecos { get; set; } = new List<EnderecoDetalheDto>();
        public ICollection<ContatoDetalheDto> Contatos { get; set; } = new List<ContatoDetalheDto>();

        public ICollection<EscolaridadeDetalheDto> Escolaridades { get; set; } = new List<EscolaridadeDetalheDto>();
        public ICollection<ExperienciaDetalheDto> Experiencias { get; set; } = new List<ExperienciaDetalheDto>();

        public static PessoaDetalheDto FromEntity(Pessoa pessoa)
        {
            return new PessoaDetalheDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                DataNascimento = pessoa.DataNascimento.Date,
                NomeMae = pessoa.NomeMae,
                NomePai = pessoa.NomePai,

                // Mapeia as coleções (de Model para DTO)
                Enderecos = pessoa.Enderecos?
                    .Select(e => EnderecoDetalheDto.FromEntity(e))
                    .ToList()
                    ?? new List<EnderecoDetalheDto>(),

                Contatos = pessoa.Contatos?
                           .Select(c => ContatoDetalheDto.FromEntity(c))
                           .ToList()
                           ?? new List<ContatoDetalheDto>(),

                Escolaridades = pessoa.Escolaridades?.Select(e => EscolaridadeDetalheDto.FromEntity(e)).ToList()
                                ?? new List<EscolaridadeDetalheDto>(),

                Experiencias = pessoa.Experiencias?.Select(exp => ExperienciaDetalheDto.FromEntity(exp)).ToList()
                               ?? new List<ExperienciaDetalheDto>()
            };
        }
    }
}
