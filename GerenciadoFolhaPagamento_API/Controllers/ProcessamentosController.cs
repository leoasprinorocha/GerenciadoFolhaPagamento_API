using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GerenciadoFolhaPagamento_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessamentosController : ControllerBase
    {
        private readonly IFuncionarioApplication _funcionarioApplication;
        private readonly IDepartamentoApplication _departamentoApplication;
        private readonly IProcessamentoFolhaApplication _processamentoFolhaApplication;

        public ProcessamentosController(IFuncionarioApplication funcionarioApplication, IDepartamentoApplication departamentoApplication, IProcessamentoFolhaApplication processamentoFolhaApplication)
        {
            _funcionarioApplication = funcionarioApplication;
            _departamentoApplication = departamentoApplication;
            _processamentoFolhaApplication = processamentoFolhaApplication;
        }

        [HttpGet]
        [Route("RecuperaTodosOsDepartamentos")]
        public async Task<IActionResult> RecuperaTodosOsDepartamentos() =>
        Ok(await _departamentoApplication.RecuperaTodosDepartamentos());

        [HttpPost]
        [Route("ExecutaProcessamento")]
        public async Task<IActionResult> ExecutaProcessamento(){
            await _processamentoFolhaApplication.IniciaProcessamento();
            return Ok(new { Resposta = "Arquivos processados com sucesso!" });
        } 
        

    }
}
