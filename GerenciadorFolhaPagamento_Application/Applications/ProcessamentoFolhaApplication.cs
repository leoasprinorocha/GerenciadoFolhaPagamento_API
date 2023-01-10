using ClosedXML.Excel;
using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Application.Applications
{
    public class ProcessamentoFolhaApplication : IProcessamentoFolhaApplication
    {

        private readonly IParametroApplication _parametroApplication;
        private readonly IDepartamentoApplication _departamentoApplication;
        private readonly IProcessamentoFolhaFuncionarioApplication _processamentoFolhaFuncionarioApplication;
        private readonly IProcessamentoFolhaRepository _processamentoFolhaRepository;
        private readonly IFuncionarioApplication _funcionarioApplication;
        private readonly IUnitOfWork _unitOfWork;
        public int tentativasExecucao = 0;
        private FileInfo[] _arquivosProcessados;

        public ProcessamentoFolhaApplication(IParametroApplication parametroApplication, IDepartamentoApplication departamentoApplication,
                                             IProcessamentoFolhaRepository processamentoFolhaRepository, IUnitOfWork unitOfWork,
                                             IProcessamentoFolhaFuncionarioApplication processamentoFolhaFuncionarioApplication,
                                             IFuncionarioApplication funcionarioApplication)
        {
            _parametroApplication = parametroApplication;
            _departamentoApplication = departamentoApplication;
            _processamentoFolhaRepository = processamentoFolhaRepository;
            _unitOfWork = unitOfWork;
            _processamentoFolhaFuncionarioApplication = processamentoFolhaFuncionarioApplication;
            _funcionarioApplication = funcionarioApplication;
        }

        public void IniciaProcessamento()
        {

            try
            {
                _unitOfWork.BeginTransaction();
                var diretorioParametro = _parametroApplication.RetornaValorParametro(1).Result;
                DirectoryInfo diretorio = new DirectoryInfo(diretorioParametro.ToString());
                FileInfo[] arquivosASeremProcessados = diretorio.GetFiles();
                _arquivosProcessados = arquivosASeremProcessados;

                foreach (var arquivo in arquivosASeremProcessados)
                {
                    DepartamentoDto departamentoProcessado = new DepartamentoDto()
                    {
                        NomeDepartamento = arquivo.Name.Split('-')[0],
                        MesVigencia = arquivo.Name.Split('-')[1],
                        AnoVigencia = arquivo.Name.Split('-')[2]
                    };
                    departamentoProcessado.AnoVigencia = departamentoProcessado.AnoVigencia.Replace(".xlsx", "");

                    var novoDepartamento = new NovoDepartamentoDto() { NomeDepartamento = departamentoProcessado.NomeDepartamento };
                    var idDepartamentoSalvo = _departamentoApplication.SalvarDepartamento(novoDepartamento).Result;


                    ProcessamentoFolha processamentoFolha = new ProcessamentoFolha();
                    var listaDeRegistros = RetornaListaDeRegistroDoArquivo(arquivo.FullName);
                    var objetoComPropriedasTotais = processamentoFolha.RetornaObjetoComPropriedadesTotais(listaDeRegistros, departamentoProcessado.MesVigencia, departamentoProcessado.AnoVigencia);
                    var quantidadeDiasUteisMes = processamentoFolha.RetornaQuantidadeDeDiasUteisDoMes(departamentoProcessado.MesVigencia, departamentoProcessado.AnoVigencia);
                    var quantidadeHorasEsperadasNoMes = quantidadeDiasUteisMes * processamentoFolha.QuantidadeDeHorasTrabalhadasEsperadaDia;

                    processamentoFolha.Departamento_idDepartamento = idDepartamentoSalvo;
                    processamentoFolha.MesVigencia = departamentoProcessado.MesVigencia;
                    processamentoFolha.TotalPagamentos = objetoComPropriedasTotais.TotalPagamentos;
                    processamentoFolha.TotalDescontos = objetoComPropriedasTotais.TotalDescontos;
                    processamentoFolha.TotalExtras = objetoComPropriedasTotais.TotalExtras;
                    processamentoFolha.AnoVigencia = departamentoProcessado.AnoVigencia;
                    var idProcessamentoFolha = SalvaProcessamentoFolha(processamentoFolha).Result;

                    List<List<RegistroPontoDto>> matrizOrdenadaPorFuncionarios = listaDeRegistros.GroupBy(x => new { x.CodigoFuncionario })
                      .Select(grp => grp.ToList()).ToList();


                    foreach (var funcionarios in matrizOrdenadaPorFuncionarios)
                    {
                        ProcessamentoFolha_Funcionario processamentoFolha_Funcionario = new ProcessamentoFolha_Funcionario();
                        processamentoFolha_Funcionario.Funcionario_Departamento_idDepartamento = idDepartamentoSalvo;
                        processamentoFolha_Funcionario.Funcionario_idFuncionario = funcionarios.First().CodigoFuncionario;
                        processamentoFolha_Funcionario.IdProcessamentoFolhaFuncionario = idProcessamentoFolha;

                        NovoFuncionarioDto novoFuncionarioDto = new NovoFuncionarioDto()
                        {
                            NomeFuncionario = funcionarios.First().Nome,
                            ValorHora = funcionarios.First().ValorHora,
                            IdDepartamento = idDepartamentoSalvo,
                            CodigoRegistroFuncionario = funcionarios.First().CodigoFuncionario
                        };

                        _funcionarioApplication.SalvarFuncionario(novoFuncionarioDto).Wait();



                        foreach (var registroPontoFuncionario in funcionarios)
                        {
                            int horasTrabalhadasMes = processamentoFolha_Funcionario.
                                                      RetornaQuantidadeTotalHorasTrabalhadasFuncionario(funcionarios.ToList());

                            if (funcionarios.IndexOf(registroPontoFuncionario) == funcionarios.Count - 1)
                            {
                                processamentoFolha_Funcionario.DiasFalta = processamentoFolha_Funcionario.RetornaQuantidadeDeDiasFaltantesMes(quantidadeDiasUteisMes, funcionarios.Count);
                                processamentoFolha_Funcionario.DiasExtras = processamentoFolha_Funcionario.RetornaQuantidadeDiasExtrasMes(quantidadeDiasUteisMes, funcionarios.Count);
                                processamentoFolha_Funcionario.DiasTrabalhados = funcionarios.Count();
                                processamentoFolha_Funcionario.TotalAReceber = (decimal)horasTrabalhadasMes * registroPontoFuncionario.ValorHora;
                                processamentoFolha_Funcionario.HorasExtras = 0;
                                processamentoFolha_Funcionario.HorasDebito = 0;

                                if (horasTrabalhadasMes < quantidadeHorasEsperadasNoMes)
                                    processamentoFolha_Funcionario.HorasDebito = quantidadeHorasEsperadasNoMes - horasTrabalhadasMes;
                                else if (horasTrabalhadasMes > quantidadeHorasEsperadasNoMes)
                                    processamentoFolha_Funcionario.HorasExtras = horasTrabalhadasMes - quantidadeHorasEsperadasNoMes;
                            }

                        }

                        _processamentoFolhaFuncionarioApplication.SalvaProcessamentoFolhaFuncionario(processamentoFolha_Funcionario).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                tentativasExecucao += 1;
                if (tentativasExecucao == 1)
                    IniciaProcessamento();

            }
            finally
            {
                _unitOfWork.Commit();
                ArquivosJaProcessados(_arquivosProcessados);
            }

        }

        public async Task<List<string>> RetornaArquivosQueEstaoNaPastaDeProcessamento()
        {
            var diretorioParametro = await _parametroApplication.RetornaValorParametro(1);
            DirectoryInfo diretorio = new DirectoryInfo(diretorioParametro.ToString());
            FileInfo[] arquivosASeremProcessados = diretorio.GetFiles();
            return arquivosASeremProcessados.Select(c => c.Name).ToList();
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


                var horaEntradaAux = Convert.ToDateTime(planilha.Cell($"E{l}").Value.ToString());
                var horaSaidaAux = Convert.ToDateTime(planilha.Cell($"F{l}").Value.ToString());
                var horaEntradaAlmocoAux = planilha.Cell($"G{l}").Value.ToString().Substring(0, 5);
                var horaSaidaAlmocoAux = planilha.Cell($"G{l}").Value.ToString().Substring(8, 5);
                var dataRegistro = DateTime.Parse(planilha.Cell($"D{l}").Value.ToString().Trim());


                RegistroPontoDto registro = new RegistroPontoDto()
                {
                    CodigoFuncionario = int.Parse(planilha.Cell($"A{l}").Value.ToString()),
                    Nome = planilha.Cell($"B{l}").Value.ToString(),
                    ValorHora = decimal.Parse(planilha.Cell($"C{l}").Value.ToString().Replace("R$", "").Replace(" ", "")),
                    DataRegistro = dataRegistro,
                    HoraEntrada = TimeSpan.Parse(horaEntradaAux.ToString("HH:mm").Trim()),
                    HoraSaida = TimeSpan.Parse(horaSaidaAux.ToString("HH:mm").Trim()),
                    HoraEntradaAlmoco = TimeSpan.Parse(horaEntradaAlmocoAux.Trim()),
                    HoraSaidaAlmoco = TimeSpan.Parse(horaSaidaAlmocoAux.Trim())
                };

                listaRegistros.Add(registro);

            }

            return listaRegistros;
        }

        private void ArquivosJaProcessados(FileInfo[] arquivosProcessados) =>
            Array.ForEach(arquivosProcessados, (c) => c.Delete());

        public async Task<List<PesquisaDepartamentosProcessadosDto>> RetornaTodosOsProcessamentos() =>
        await _processamentoFolhaRepository.PesquisaDepartamentosProcessados();

        public void LimparDadosProcessados()
        {
            try
            {
                _unitOfWork.BeginTransaction();
                _processamentoFolhaRepository.LimparDadosProcessados();
            }
            finally
            {

                _unitOfWork.Commit();
            }
        }


    }
}
