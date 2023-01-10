using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GerenciadoFolhaPagamento_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessamentosController : ControllerBase
    {
        private readonly IDepartamentoApplication _departamentoApplication;
        private readonly IProcessamentoFolhaApplication _processamentoFolhaApplication;

        public ProcessamentosController(IDepartamentoApplication departamentoApplication,
                                        IProcessamentoFolhaApplication processamentoFolhaApplication)
        {

            _departamentoApplication = departamentoApplication;
            _processamentoFolhaApplication = processamentoFolhaApplication;
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
            try
            {
                var listaDepartamentosProcessados = await _processamentoFolhaApplication.RetornaTodosOsProcessamentos();
                return Ok(JsonConvert.SerializeObject(listaDepartamentosProcessados).ToString());
            }
            catch (Exception ex)
            {

                throw;
            }
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
