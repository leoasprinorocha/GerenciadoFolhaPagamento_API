
using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace GerenciadorFolhaPagamento_Domain.Entities
{
    public class ProcessamentoFolha
    {
        public int IdProcessamentoFolha { get; set; }

        public int Departamento_idDepartamento { get; set; }

        public string MesVigencia { get; set; }
        public string AnoVigencia { get; set; }

        public decimal? TotalPagamentos { get; set; }

        public decimal? TotalDescontos { get; set; }

        public decimal? TotalExtras { get; set; }


        public int RetornaQuantidadeDeDiasUteisDoMes(string mes, string ano)
        {
            int diasUteis = 0;
            MesEnum mesInteiro = (MesEnum)Enum.Parse(typeof(MesEnum), mes);
            DateTime primeiroDiaDoMes = new DateTime(Convert.ToInt32(ano), (int)mesInteiro, 1);
            DateTime ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddDays(-1);

            while (primeiroDiaDoMes.Date <= ultimoDiaDoMes.Date)
            {
                if (primeiroDiaDoMes.DayOfWeek != DayOfWeek.Saturday
                   && primeiroDiaDoMes.DayOfWeek != DayOfWeek.Sunday)
                    diasUteis++;

                primeiroDiaDoMes = primeiroDiaDoMes.AddDays(1);
            }

            return diasUteis;

        }

        public ProcessamentoFolhaDto RetornaObjetoComPropriedadesTotais(List<RegistroPontoDto> listaRegistros, string mes, string ano)
        {
           
            int totalDeHorasEsperadasMes = (RetornaQuantidadeDeDiasUteisDoMes(mes, ano)) * QuantidadeDeHorasTrabalhadasEsperadaDia;
           
            List<List<RegistroPontoDto>> matrizOrdenadaPorFuncionarios = listaRegistros.GroupBy(x => new { x.CodigoFuncionario })
            .Select(grp => grp.ToList()).ToList();

            List<ProcessamentoFolhaDto> listasTotaisFuncionarios = new List<ProcessamentoFolhaDto>();

            foreach (var registros in matrizOrdenadaPorFuncionarios)
            {

                decimal totalDescontosFunc = 0;
                decimal totalExtrasFunc = 0;
                decimal totalPagamentosFunc = 0;
                double totalHorasTrabalhadasFunc = 0;

                foreach (var registro in registros)
                {
                    var horasTrabalhadasDia = (registro.HoraSaida - registro.HoraEntrada) - (registro.HoraSaidaAlmoco - registro.HoraEntradaAlmoco);
                    totalHorasTrabalhadasFunc += horasTrabalhadasDia.TotalHours;
                    totalPagamentosFunc += Convert.ToDecimal((horasTrabalhadasDia.TotalHours)) * (registro.ValorHora);

                    if (horasTrabalhadasDia.TotalHours < QuantidadeDeHorasTrabalhadasEsperadaDia)
                    {
                        totalDescontosFunc += Convert.ToDecimal((QuantidadeDeHorasTrabalhadasEsperadaDia - horasTrabalhadasDia.TotalHours)) * (registro.ValorHora);
                    }
                    else if (horasTrabalhadasDia.TotalHours > QuantidadeDeHorasDeAlmocoEsperadaDia)
                    {
                        totalExtrasFunc += Convert.ToDecimal((horasTrabalhadasDia.TotalHours - QuantidadeDeHorasTrabalhadasEsperadaDia)) * (registro.ValorHora);
                    }


                    if (registros.IndexOf(registro) == registros.Count - 1)
                    {
                        ProcessamentoFolhaDto processamentoFuncionario = new ProcessamentoFolhaDto()
                        {
                            TotalPagamentos = totalPagamentosFunc,
                            TotalHorasTrabalhadas = (int)totalHorasTrabalhadasFunc,
                            TotalDescontos = totalDescontosFunc,
                            TotalExtras = totalExtrasFunc
                        };

                        if (totalHorasTrabalhadasFunc < totalDeHorasEsperadasMes)
                        {
                            
                            processamentoFuncionario.TotalDescontos = ((int)(totalDeHorasEsperadasMes - totalHorasTrabalhadasFunc)) * registro.ValorHora;
                            processamentoFuncionario.TotalExtras = 0;
                        }
                        else if (totalHorasTrabalhadasFunc > totalDeHorasEsperadasMes)
                        {
                            processamentoFuncionario.TotalExtras = ((int)(totalHorasTrabalhadasFunc - totalDeHorasEsperadasMes)) * registro.ValorHora;
                            processamentoFuncionario.TotalDescontos = 0;
                        }

                        listasTotaisFuncionarios.Add(processamentoFuncionario);

                    }

                }

            }

            ProcessamentoFolhaDto processamentoFolhaDto = new ProcessamentoFolhaDto()
            {
                TotalPagamentos = listasTotaisFuncionarios.Sum(c => c.TotalPagamentos),
                TotalDescontos = listasTotaisFuncionarios.Sum(c => c.TotalDescontos),
                TotalExtras = listasTotaisFuncionarios.Sum(c => c.TotalExtras),
            };

            return processamentoFolhaDto;
        }

        public int QuantidadeDeHorasTrabalhadasEsperadaDia { get { return 8; } }
        public int QuantidadeDeHorasDeAlmocoEsperadaDia { get { return 1; } }
    }
}
