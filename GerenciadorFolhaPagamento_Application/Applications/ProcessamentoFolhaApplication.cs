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

                    processamentoFolha.Departamento_idDepartamento = idDepartamentoSalvo;
                    processamentoFolha.MesVigencia = departamentoProcessado.MesVigencia;
                    processamentoFolha.TotalPagamentos = objetoComPropriedasTotais.TotalPagamentos;
                    processamentoFolha.TotalDescontos = objetoComPropriedasTotais.TotalDescontos;
                    processamentoFolha.TotalExtras = objetoComPropriedasTotais.TotalHorasExtras;
                    processamentoFolha.AnoVigencia = departamentoProcessado.AnoVigencia;
                    var idProcessamentoFolha = SalvaProcessamentoFolha(processamentoFolha).Result;


                    foreach (var registroPontoFuncionario in listaDeRegistros)
                    {
                        ProcessamentoFolha_Funcionario processamentoFolha_Funcionario = new ProcessamentoFolha_Funcionario();
                        processamentoFolha_Funcionario.Funcionario_Departamento_idDepartamento = idDepartamentoSalvo;
                        processamentoFolha_Funcionario.Funcionario_idFuncionario = registroPontoFuncionario.CodigoFuncionario;
                        processamentoFolha_Funcionario.IdProcessamentoFolhaFuncionario = idProcessamentoFolha;

                        NovoFuncionarioDto novoFuncionarioDto = new NovoFuncionarioDto()
                        {
                            NomeFuncionario = registroPontoFuncionario.Nome,
                            ValorHora = registroPontoFuncionario.ValorHora,
                            IdDepartamento = idDepartamentoSalvo,
                            CodigoRegistroFuncionario = registroPontoFuncionario.CodigoFuncionario
                        };

                        _funcionarioApplication.SalvarFuncionario(novoFuncionarioDto).Wait();


                        int horasTrabalhadasMes = processamentoFolha_Funcionario.
                                                  RetornaQuantidadeTotalHorasTrabalhadasFuncionario(listaDeRegistros.Where(c => c.CodigoFuncionario == registroPontoFuncionario.CodigoFuncionario).ToList());

                        int horasTrabalhadasDia = processamentoFolha_Funcionario.RetornaQuantidadeHorasTrabalhadasDia(registroPontoFuncionario);

                        processamentoFolha_Funcionario.TotalAReceber = processamentoFolha_Funcionario.RetornaTotalAReceber(registroPontoFuncionario.ValorHora, horasTrabalhadasDia);
                        processamentoFolha_Funcionario.HorasExtras = horasTrabalhadasDia > processamentoFolha.QuantidadeDeHorasTrabalhadasEsperadaDia ? (horasTrabalhadasDia - processamentoFolha.QuantidadeDeHorasTrabalhadasEsperadaDia) : 0;
                        processamentoFolha_Funcionario.HorasDebito = processamentoFolha.QuantidadeDeHorasTrabalhadasEsperadaDia > horasTrabalhadasDia ? (processamentoFolha.QuantidadeDeHorasTrabalhadasEsperadaDia - horasTrabalhadasDia) : 0;

                        int quantidadeDiasTrabalhadosMes = listaDeRegistros.Where(c => c.CodigoFuncionario == registroPontoFuncionario.CodigoFuncionario).Count();

                        processamentoFolha_Funcionario.DiasFalta = processamentoFolha_Funcionario.RetornaQuantidadeDeDiasFaltantesMes(quantidadeDiasUteisMes, quantidadeDiasTrabalhadosMes);
                        processamentoFolha_Funcionario.DiasExtras = processamentoFolha_Funcionario.RetornaQuantidadeDiasExtrasMes(quantidadeDiasUteisMes, quantidadeDiasTrabalhadosMes);
                        processamentoFolha_Funcionario.DiasTrabalhados = quantidadeDiasTrabalhadosMes;
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


                RegistroPontoDto registro = new RegistroPontoDto()
                {
                    CodigoFuncionario = int.Parse(planilha.Cell($"A{l}").Value.ToString()),
                    Nome = planilha.Cell($"B{l}").Value.ToString(),
                    ValorHora = decimal.Parse(planilha.Cell($"C{l}").Value.ToString().Replace("R$", "").Replace(" ", "")),
                    DataRegistro = DateTime.Parse(planilha.Cell($"D{l}").Value.ToString()),
                    HoraEntrada = TimeSpan.Parse(horaEntradaAux.ToString("HH:mm")),
                    HoraSaida = TimeSpan.Parse(horaSaidaAux.ToString("HH:mm")),
                    HoraEntradaAlmoco = TimeSpan.Parse(horaEntradaAlmocoAux),
                    HoraSaidaAlmoco = TimeSpan.Parse(horaSaidaAlmocoAux)
                };

                listaRegistros.Add(registro);

            }

            return listaRegistros;
        }

        private void ArquivosJaProcessados(FileInfo[] arquivosProcessados) =>
            Array.ForEach(arquivosProcessados, (c) => c.Delete());

        public async Task<List<PesquisaDepartamentosProcessadosDto>> RetornaTodosOsProcessamentos() =>
        await _processamentoFolhaRepository.PesquisaDepartamentosProcessados();

    }
}
