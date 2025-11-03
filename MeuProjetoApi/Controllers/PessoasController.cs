using MeuProjetoApi.DTOs;
using MeuProjetoApi.Models;
using MeuProjetoApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MeuProjetoApi.Controllers
{
    [Route("api/pessoas")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;
        public PessoasController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CriarPessoa([FromBody] PessoaCriacaoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Pessoa novaPessoa = await _pessoaService.CriarPessoaAsync(dto);

                return CreatedAtAction(
                        nameof(CriarPessoa),
                        new { id = novaPessoa.Id },
                        novaPessoa
                    );
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("CPF já cadastrado"))
            {
                return Conflict(new { message = ex.Message});
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new
                {
                    message = "Ocorreu um erro interno ao processar a requisição.",
                    details = ex.Message
                });
            }
        }
    }
}
