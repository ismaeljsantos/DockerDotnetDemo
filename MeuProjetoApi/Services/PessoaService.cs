// Arquivo: MeuProjetoApi/Services/PessoaService.cs (Conteúdo Corrigido)

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

            // --- VALIDAÇÕES DE LIMITE (mantidas) ---
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

                if (dto.Contatos.Count(c => (c.Tipo.Equals("Telefone", StringComparison.OrdinalIgnoreCase) || c.Tipo.Equals("Celular", StringComparison.OrdinalIgnoreCase)) && c.IsPrincipal) > 1)
                    throw new InvalidOperationException("Regra de Prioridade: Apenas um contato de telefone pode ser definido como Principal.");

                if (dto.Contatos.Count(c => c.Tipo.Equals("Email", StringComparison.OrdinalIgnoreCase) && c.IsPrincipal) > 1)
                    throw new InvalidOperationException("Regra de Prioridade: Apenas um contato de e-mail pode ser definido como Principal.");
            }

            if (dto.Enderecos != null && dto.Enderecos.Count(e => e.IsPrincipal) > 1)
                throw new InvalidOperationException("Regra de Negócio: Apenas um endereço pode ser marcado como principal.");


            string cpfLimpo = dto.Cpf;

            // --- SEGURANÇA E UNICIDADE (mantidas) ---
            string cpfHash = _cpfSecurity.GerarHash(cpfLimpo);
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
            var pessoaId = novaPessoa.Id;

            // --- COLEÇÕES ANINHADAS (Endereços e Contatos) ---
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
                    IsPrincipal = eDto.IsPrincipal,
                    FkPessoaId = pessoaId // JÁ ESTAVA CORRETO
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
                    IsPrincipal = cDto.IsPrincipal,
                    FkPessoaId = pessoaId // JÁ ESTAVA CORRETO
                }).ToList();
                novaPessoa.Contatos = contatos;
            }

            // --- ESCOLARIDADE (UPSERT) ---
            if (dto.Escolaridades != null && dto.Escolaridades.Any())
            {
                foreach (var eDto in dto.Escolaridades)
                {
                    var instituicao = await GetOrCreateInstituicaoAsync(eDto.NomeInstituicao);

                    // Validação de Obrigatoriedade
                    if (eDto.Tipo.Equals("Superior", StringComparison.OrdinalIgnoreCase) ||
                        eDto.Tipo.Equals("Profissionalizante", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!eDto.AnoInicio.HasValue || !eDto.AnoConclusao.HasValue)
                        {
                            throw new InvalidOperationException($"Regra de Negócio: Para o tipo {eDto.Tipo}, os campos AnoInicio e AnoConclusao são obrigatórios.");
                        }
                    }

                    // 🛑 CORREÇÃO: Adiciona diretamente, e AGORA COM O FkPessoaId
                    novaPessoa.Escolaridades.Add(new Escolaridade
                    {
                        Instituicao = instituicao,
                        FkPessoaId = pessoaId, // <--- NOVA CORREÇÃO APLICADA AQUI
                        Tipo = eDto.Tipo,
                        NomeCurso = eDto.NomeCurso,
                        AnoInicio = eDto.AnoInicio,
                        AnoConclusao = eDto.AnoConclusao,
                        Ativo = true
                    });
                }
            }

            // --- EXPERIÊNCIA PROFISSIONAL (UPSERT) ---
            if (dto.Experiencias != null && dto.Experiencias.Any())
            {
                foreach (var expDto in dto.Experiencias)
                {
                    var empresa = await GetOrCreateEmpresaAsync(expDto.NomeEmpresa);

                    // 🛑 CORREÇÃO: Adiciona diretamente, e AGORA COM O FkPessoaId
                    novaPessoa.Experiencias.Add(new ExperienciaProfissional
                    {
                        Empresa = empresa,
                        FkPessoaId = pessoaId, // <--- NOVA CORREÇÃO APLICADA AQUI
                        Funcao = expDto.Funcao,
                        AnoEntrada = expDto.AnoEntrada,
                        AnoSaida = expDto.AnoSaida,
                    });
                }
            }

            // --- SALVAR TUDO ---
            _context.Pessoas.Add(novaPessoa);
            await _context.SaveChangesAsync(); // Agora deve funcionar, pois todas as FKs estão setadas

            return novaPessoa;
        }

        // --- MÉTODOS DE UPSERT (Já corrigidos com ToLower() no seu código) ---

        private async Task<Instituicao> GetOrCreateInstituicaoAsync(string nomeInstituicao)
        {
            var nomeInstituicaoLower = nomeInstituicao.ToLower();

            var instituicao = await _context.Instituicoes
                                            .FirstOrDefaultAsync(i => i.Nome.ToLower() == nomeInstituicaoLower);

            if (instituicao != null) return instituicao;

            return new Instituicao { Nome = nomeInstituicao };
        }

        private async Task<Empresa> GetOrCreateEmpresaAsync(string nomeEmpresa)
        {
            var nomeEmpresaLower = nomeEmpresa.ToLower();

            var empresa = await _context.Empresas
                                         .FirstOrDefaultAsync(e => e.Nome.ToLower() == nomeEmpresaLower);

            if (empresa != null) return empresa;

            return new Empresa { Nome = nomeEmpresa };
        }
    }
}