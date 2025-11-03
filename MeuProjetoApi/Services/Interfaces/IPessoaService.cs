using MeuProjetoApi.DTOs;
using MeuProjetoApi.Models;
using System.Threading.Tasks;

namespace MeuProjetoApi.Services.Interfaces
{
    public interface IPessoaService
    {
        /// <summary>
        /// Cria uma nova Pessoa, realizando hash e criptografia do CPF e garantindo a unicidade
        /// </summary>
        /// <param name="dto">Dados da pessoa a ser criada (incluindo CPF em texto limpo)</param>
        /// <returns>A entidade Pessoa criada</returns>
        Task<Pessoa> CriarPessoaAsync(PessoaCriacaoDto dto);
    }
}
