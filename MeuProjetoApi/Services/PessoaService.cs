using MeuProjetoApi.Data;
using MeuProjetoApi.DTOs;
using MeuProjetoApi.Models;
using MeuProjetoApi.Services.Interfaces;
using MeuProjetoApi.Services.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq; // Necessário para Any() e Count()
using System.Threading.Tasks;

namespace MeuProjetoApi.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly AppDbContext _context;
        private readonly CpfSecurity _cpfSecurity;

        public PessoaService(AppDbContext context, CpfSecurity cpfSecurity)
        {
            _context = context;
            _cpfSecurity = cpfSecurity;
        }

        public async Task<Pessoa> CriarPessoaAsync(PessoaCriacaoDto dto)
        {
            const int MaximoEnderecos = 2;
            const int MaximoTelefones = 2;
            const int MaximoEmail = 2;

            // --- VALIDAÇÕES DE LIMITE ---
            if (dto.Enderecos != null && dto.Enderecos.Count > MaximoEnderecos)
                throw new InvalidOperationException($"Regra de Negócio: O número máximo de endereços permitidos é {MaximoEnderecos}");

            if (dto.Contatos != null && dto.Contatos.Any())
            {
                int countTelefones = dto.Contatos.Count(c => c.Tipo.Equals("Telefone", StringComparison.OrdinalIgnoreCase) || c.Tipo.Equals("Celular", StringComparison.OrdinalIgnoreCase));
                int countEmails = dto.Contatos.Count(c => c.Tipo.Equals("Email", StringComparison.OrdinalIgnoreCase));

                if (countTelefones > MaximoTelefones)
                    throw new InvalidOperationException($"Regra de Negócio: O número máximo de Telefones permitidos é {MaximoTelefones}");

                if (countEmails > MaximoEmail)
                    throw new InvalidOperationException($"Regra de Negócio: O número máximo de Emails permitidos é {MaximoEmail}");

                // Validação de Prioridade (Apenas um Principal por Tipo)
                if (dto.Contatos.Count(c => (c.Tipo.Equals("Telefone", StringComparison.OrdinalIgnoreCase) || c.Tipo.Equals("Celular", StringComparison.OrdinalIgnoreCase)) && c.IsPrincipal) > 1)
                    throw new InvalidOperationException("Regra de Prioridade: Apenas um contato de telefone pode ser definido como Principal.");

                if (dto.Contatos.Count(c => c.Tipo.Equals("Email", StringComparison.OrdinalIgnoreCase) && c.IsPrincipal) > 1)
                    throw new InvalidOperationException("Regra de Prioridade: Apenas um contato de e-mail pode ser definido como Principal.");
            }

            if (dto.Enderecos != null && dto.Enderecos.Count(e => e.IsPrincipal) > 1)
                throw new InvalidOperationException("Regra de Negócio: Apenas um endereço pode ser marcado como principal.");


            string cpfLimpo = dto.Cpf;

            // --- SEGURANÇA E UNICIDADE ---
            string cpfHash = _cpfSecurity.GerarHash(cpfLimpo); // Uso estático é aceitável, mas pode ser ajustado
            string cpfCriptografado = _cpfSecurity.Criptografar(cpfLimpo);

            var cpfExistente = await _context.Pessoas.AsNoTracking().FirstOrDefaultAsync(p => p.CpfHash == cpfHash);

            if (cpfExistente != null)
                throw new InvalidOperationException("CPF já cadastrado no sistema.");

            // --- MAPEAMENTO PRINCIPAL ---
            var novaPessoa = new Pessoa
            {
                Nome = dto.Nome,
                DataNascimento = dto.DataNascimento,
                NomeMae = dto.NomeMae,
                NomePai = dto.NomePai ?? string.Empty,
                CpfHash = cpfHash,
                CpfCriptografado = cpfCriptografado,
            };

            // --- COLEÇÕES ANINHADAS ---
            if (dto.Enderecos != null && dto.Enderecos.Any())
            {
                var enderecos = dto.Enderecos.Select(eDto => new Endereco
                {
                    Logradouro = eDto.Logradouro,
                    Numero = eDto.Numero,
                    Complemento = eDto.Complemento,
                    Bairro = eDto.Bairro,
                    Cidade = eDto.Cidade,
                    Estado = eDto.Estado,
                    Cep = eDto.Cep,
                    IsPrincipal = eDto.IsPrincipal
                }).ToList();
                novaPessoa.Enderecos = enderecos;
            }

            if (dto.Contatos != null && dto.Contatos.Any())
            {
                var contatos = dto.Contatos.Select(cDto => new Contato
                {
                    Tipo = cDto.Tipo,
                    Valor = cDto.Valor,
                    Observacao = cDto.Observacao,
                    IsPrincipal = cDto.IsPrincipal
                }).ToList();
                novaPessoa.Contatos = contatos;
            }

            // --- ESCOLARIDADE (UPSERT/VALICAÇÃO) ---
            if (dto.Escolaridades != null && dto.Escolaridades.Any())
            {
                var escolaridadesTemporarias = new List<Escolaridade>(); // Lista temporária
                foreach (var eDto in dto.Escolaridades)
                {
                    var instituicao = await GetOrCreateInstituicaoAsync(eDto.NomeInstituicao);

                    // ... (Validação de Obrigatoriedade) ...

                    escolaridadesTemporarias.Add(new Escolaridade
                    {
                        Instituicao = instituicao,
                        Tipo = eDto.Tipo,
                        NomeCurso = eDto.NomeCurso,
                        AnoInicio = eDto.AnoInicio,
                        AnoConclusao = eDto.AnoConclusao,
                        Ativo = true
                    });
                }

                // 🛑 CORREÇÃO: Usar AddRange na coleção da novaPessoa
                ((List<Escolaridade>)novaPessoa.Escolaridades).AddRange(escolaridadesTemporarias);

                // NOTA: Se novaPessoa.Escolaridades for ICollection<T> = new List<T>(), este AddRange é seguro.
            }

            // --- EXPERIÊNCIA PROFISSIONAL (UPSERT) ---
            if (dto.Experiencias != null && dto.Experiencias.Any())
            {
                var experienciasTemporarias = new List<ExperienciaProfissional>(); // Lista temporária
                foreach (var expDto in dto.Experiencias)
                {
                    var empresa = await GetOrCreateEmpresaAsync(expDto.NomeEmpresa);
                    experienciasTemporarias.Add(new ExperienciaProfissional
                    {
                        Empresa = empresa,
                        Funcao = expDto.Funcao,
                        AnoEntrada = expDto.AnoEntrada,
                        AnoSaida = expDto.AnoSaida,
                    });
                }

                // 🛑 CORREÇÃO: Usar AddRange na coleção da novaPessoa
                ((List<ExperienciaProfissional>)novaPessoa.Experiencias).AddRange(experienciasTemporarias);
            }

            // --- SALVAR TUDO ---
            _context.Pessoas.Add(novaPessoa);
            await _context.SaveChangesAsync();

            return novaPessoa;
        }

        // --- MÉTODOS DE UPSERT ---

        private async Task<Instituicao> GetOrCreateInstituicaoAsync(string nomeInstituicao)
        {
            var instituicao = await _context.Instituicoes
                                            .FirstOrDefaultAsync(i => i.Nome.Equals(nomeInstituicao, StringComparison.OrdinalIgnoreCase));
            if (instituicao != null) return instituicao;

            return new Instituicao { Nome = nomeInstituicao };
        }

        private async Task<Empresa> GetOrCreateEmpresaAsync(string nomeEmpresa)
        {
            var empresa = await _context.Empresas
                                         .FirstOrDefaultAsync(e => e.Nome.Equals(nomeEmpresa, StringComparison.OrdinalIgnoreCase));
            if (empresa != null) return empresa;

            return new Empresa { Nome = nomeEmpresa };
        }
    }
}