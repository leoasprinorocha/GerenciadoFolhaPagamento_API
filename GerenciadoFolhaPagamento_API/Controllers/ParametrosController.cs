using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadoFolhaPagamento_API.Controllers
{
    [ApiController]
    [Route("Parametros")]
    public class ParametrosController : ControllerBase
    {
        private readonly IParametroApplication _parametroApplication;
        public ParametrosController(IParametroApplication parametroApplication)
        {
            _parametroApplication = parametroApplication;
        }

        [HttpPost]
        [Route("CadastrarParametro")]
        public async Task<IActionResult> CadastrarParametro(List<NovoParametroDto> novoParametroDto)
        {
            await _parametroApplication.SalvaNovoParametro(novoParametroDto);
            return Ok(new { Resposta = "Parâmetro cadastrado com sucesso!" });
        }

        [HttpGet]
        [Route("RetornaTodosOsParametros")]
        public async Task<IActionResult> RetornaTodosOsParametros() =>
        Ok(await _parametroApplication.RetornaTodosOsParametros());

    }
}
