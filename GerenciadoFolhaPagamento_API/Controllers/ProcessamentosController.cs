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

        public ProcessamentosController(IFuncionarioApplication funcionarioApplication, IDepartamentoApplication departamentoApplication)
        {
            _funcionarioApplication = funcionarioApplication;
            _departamentoApplication = departamentoApplication;
        }

        [HttpGet]
        [Route("RecuperaTodosOsDepartamentos")]
        public async Task<IActionResult> RecuperaTodosOsDepartamentos() =>
        Ok(await _departamentoApplication.RecuperaTodosDepartamentos());


    }
}
