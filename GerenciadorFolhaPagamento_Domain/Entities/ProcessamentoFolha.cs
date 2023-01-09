
using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Enums;
using System;
using System.Collections.Generic;
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
            TimeSpan totalHoras = TimeSpan.Zero;
            int totalHorasExtras = 0;
            int totalHorasNegativas = 0;
            decimal totalPagamentos = 0;
            int totalDeHorasEsperadasMes = (RetornaQuantidadeDeDiasUteisDoMes(mes, ano)) * QuantidadeDeHorasTrabalhadasEsperadaDia;
            decimal totalDescontos = 0;
            decimal totalExtras = 0;


            foreach (var registro in listaRegistros)
            {
                var horasTrabalhadasDia = (registro.HoraSaida - registro.HoraEntrada) - (registro.HoraSaidaAlmoco - registro.HoraEntradaAlmoco);
                var valorPagoDia = Convert.ToDecimal((horasTrabalhadasDia.TotalHours)) * (registro.ValorHora);

                if (horasTrabalhadasDia.TotalHours < QuantidadeDeHorasTrabalhadasEsperadaDia)
                {
                    totalDescontos += Convert.ToDecimal((QuantidadeDeHorasTrabalhadasEsperadaDia - horasTrabalhadasDia.TotalHours)) * (registro.ValorHora);
                    totalHorasNegativas += QuantidadeDeHorasTrabalhadasEsperadaDia - Convert.ToInt32(horasTrabalhadasDia.TotalHours);
                }
                else if (horasTrabalhadasDia.TotalHours > QuantidadeDeHorasDeAlmocoEsperadaDia)
                {
                    totalExtras += Convert.ToDecimal((horasTrabalhadasDia.TotalHours - QuantidadeDeHorasTrabalhadasEsperadaDia)) * (registro.ValorHora);
                    totalHorasExtras += Convert.ToInt32(horasTrabalhadasDia.TotalHours) - QuantidadeDeHorasTrabalhadasEsperadaDia;
                }

                totalHoras += horasTrabalhadasDia;
                totalPagamentos += valorPagoDia;

            }

            ProcessamentoFolhaDto processamentoFolhaDto = new ProcessamentoFolhaDto()
            {
                TotalPagamentos = totalPagamentos,
                TotalHorasTrabalhadas = (int)totalHoras.TotalHours,
                TotalDescontos = totalDescontos,
                TotalHorasExtras = totalHorasExtras,
                TotalHorasNegativas = totalHorasNegativas,
                TotalHorasEsperadasMes = totalDeHorasEsperadasMes
            };

            return processamentoFolhaDto;
        }

        public int QuantidadeDeHorasTrabalhadasEsperadaDia { get { return 8; } }
        public int QuantidadeDeHorasDeAlmocoEsperadaDia { get { return 1; } }
    }
}
