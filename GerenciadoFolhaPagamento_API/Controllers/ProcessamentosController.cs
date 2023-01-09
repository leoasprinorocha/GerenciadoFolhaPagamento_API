using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IUnitOfWork _unitOfWork;

        public ProcessamentosController(IFuncionarioApplication funcionarioApplication, IDepartamentoApplication departamentoApplication, 
                                        IProcessamentoFolhaApplication processamentoFolhaApplication, IUnitOfWork unitOfWork)
        {
            _funcionarioApplication = funcionarioApplication;
            _departamentoApplication = departamentoApplication;
            _processamentoFolhaApplication = processamentoFolhaApplication;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("RecuperaTodosOsDepartamentos")]
        public async Task<IActionResult> RecuperaTodosOsDepartamentos() =>
        Ok(await _departamentoApplication.RecuperaTodosDepartamentos());

        [HttpPost]
        [Route("ExecutaProcessamento")]
        public IActionResult ExecutaProcessamento()
        {
            Task.Run(() => _processamentoFolhaApplication.IniciaProcessamento());
            return Ok(new { Resposta = "Processamento iniciado com sucesso!" });
        }

        [HttpGet]
        [Route("RetornaArquivosQueEstaoNaPastaDeProcessamento")]
        public async Task<IActionResult> RetornaArquivosQueEstaoNaPastaDeProcessamento()
        {
            var listaArquivos = await _processamentoFolhaApplication.RetornaArquivosQueEstaoNaPastaDeProcessamento();
            return Ok(listaArquivos);
        }

        [HttpGet]
        [Route("RetornaDepartamentosProcessados")]
        public async Task<IActionResult> RetornaDepartamentosProcessados()
        {
            var listaDepartamentosProcessados = await _processamentoFolhaApplication.RetornaTodosOsProcessamentos();
            return Ok(JsonConvert.SerializeObject(listaDepartamentosProcessados).ToString());
        }

        [HttpPost]
        [Route("LimparDadosProcessados")]
        public IActionResult LimparDadosProcessados()
        {
            _processamentoFolhaApplication.LimparDadosProcessados();
            return Ok(new { Resposta = "Limpeza feita com sucesso!" });
        }


    }
}
