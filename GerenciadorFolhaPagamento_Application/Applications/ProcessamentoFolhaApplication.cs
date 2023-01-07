using ClosedXML.Excel;
using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Application.Applications
{
    public class ProcessamentoFolhaApplication : IProcessamentoFolhaApplication
    {

        private readonly IParametroApplication _parametroApplication;
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IProcessamentoFolhaRepository _processamentoFolhaRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessamentoFolhaApplication(IParametroApplication parametroApplication, IDepartamentoRepository departamentoRepository, 
                                             IProcessamentoFolhaRepository processamentoFolhaRepository, IUnitOfWork unitOfWork)
        {
            _parametroApplication = parametroApplication;
            _departamentoRepository = departamentoRepository;
            _processamentoFolhaRepository = processamentoFolhaRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task IniciaProcessamento()
        {

            string[] diretorio = Directory.GetDirectories((string)await _parametroApplication.RetornaValorParametro(1));
            string[] arquivosASeremProcessados = Directory.GetDirectories(diretorio[0]);

            foreach (var arquivo in arquivosASeremProcessados)
            {
                DepartamentoDto departamentoProcessado = new DepartamentoDto()
                {
                    NomeDepartamento = arquivo.Split('-')[0],
                    MesVigencia = arquivo.Split('-')[1],
                    AnoVigencia = arquivo.Split('-')[2]
                };

                ProcessamentoFolha processamentoFolha = new ProcessamentoFolha();
                var listaDeRegistros = RetornaListaDeRegistroDoArquivo(arquivo);
                var objetoComPropriedasTotais = processamentoFolha.RetornaObjetoComPropriedadesTotais(listaDeRegistros, departamentoProcessado.MesVigencia, departamentoProcessado.AnoVigencia);

                processamentoFolha.Departamento_idDepartamento = await _departamentoRepository.RetornaIdDepartamentoPeloNome(departamentoProcessado.NomeDepartamento),
                processamentoFolha.MesVigencia = departamentoProcessado.MesVigencia;
                processamentoFolha.TotalPagamentos = objetoComPropriedasTotais.TotalPagamentos;
                processamentoFolha.TotalDescontos = objetoComPropriedasTotais.TotalDescontos;
                processamentoFolha.TotalExtras = objetoComPropriedasTotais.TotalHorasExtras;

                _unitOfWork.BeginTransaction();
                var idProcessamentoFolha = await SalvaProcessamentoFolha(processamentoFolha);
                _unitOfWork.Commit();



            }

        }

        public async Task<int> SalvaProcessamentoFolha(ProcessamentoFolha processamentoFolha) =>
        await _processamentoFolhaRepository.SalvaProcessamentoFolha(processamentoFolha);


        private List<RegistroPontoDto> RetornaListaDeRegistroDoArquivo(string caminhoArquivo)
        {
            var xls = new XLWorkbook(caminhoArquivo);
            List<RegistroPontoDto> listaRegistros = new List<RegistroPontoDto>();
            var planilha = xls.Worksheets.First();
            var totalLinhas = planilha.Rows().Count();
            for (int l = 2; l <= totalLinhas; l++)
            {
                var codigoFuncionario = int.Parse(planilha.Cell($"A{l}").Value.ToString());
                var nomeFuncionario = planilha.Cell($"B{l}").Value.ToString();
                var valorHora = decimal.Parse(planilha.Cell($"C{l}").Value.ToString().Replace("R$", "").Replace(",", ".").Replace(" ", ""));
                var dataRegistroPonto = DateTime.Parse(planilha.Cell($"D{l}").Value.ToString());
                var horaEntrada = TimeSpan.Parse(planilha.Cell($"E{l}").Value.ToString());
                var horaSaida = TimeSpan.Parse(planilha.Cell($"F{l}").Value.ToString());
                var horaEntradaAlmoco = TimeSpan.Parse(planilha.Cell($"G{l}").Value.ToString().Substring(0, 5));
                var horaSaidaAlmoco = TimeSpan.Parse(planilha.Cell($"G{l}").Value.ToString().Substring(8, 5));

                RegistroPontoDto registro = new RegistroPontoDto()
                {
                    CodigoFuncionario = int.Parse(planilha.Cell($"A{l}").Value.ToString()),
                    Nome = planilha.Cell($"B{l}").Value.ToString(),
                    ValorHora = decimal.Parse(planilha.Cell($"C{l}").Value.ToString().Replace("R$", "").Replace(",", ".").Replace(" ", "")),
                    DataRegistro = DateTime.Parse(planilha.Cell($"D{l}").Value.ToString()),
                    HoraEntrada = TimeSpan.Parse(planilha.Cell($"E{l}").Value.ToString()),
                    HoraSaida = TimeSpan.Parse(planilha.Cell($"F{l}").Value.ToString()),
                    HoraEntradaAlmoco = TimeSpan.Parse(planilha.Cell($"G{l}").Value.ToString().Substring(0, 5)),
                    HoraSaidaAlmoco = TimeSpan.Parse(planilha.Cell($"G{l}").Value.ToString().Substring(8, 5))
                };

                listaRegistros.Add(registro);

            }

            return listaRegistros;
        }
    }
}
