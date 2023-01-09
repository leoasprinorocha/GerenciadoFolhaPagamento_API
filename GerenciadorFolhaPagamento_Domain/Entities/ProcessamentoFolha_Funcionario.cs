

using GerenciadorFolhaPagamento_Domain.Dtos;
using System;
using System.Collections.Generic;

namespace GerenciadorFolhaPagamento_Domain.Entities
{
    public class ProcessamentoFolha_Funcionario
    {
        public int IdProcessamentoFolhaFuncionario { get; set; }

        public int Funcionario_Departamento_idDepartamento { get; set; }

        public int Funcionario_idFuncionario { get; set; }

        public int ProcessamentoFolha_idProcessamentoFolha { get; set; }

        public decimal? TotalAReceber { get; set; }

        public decimal HorasExtras { get; set; }

        public decimal HorasDebito { get; set; }

        public int? DiasFalta { get; set; }

        public int? DiasExtras { get; set; }

        public int? DiasTrabalhados { get; set; }

        public decimal RetornasHorasNegativasMes(int diasUteisNoMes, int horasEsperadasDeTrabalhoDia, int horasTrabalhadasMes)
        {
            TimeSpan totalHorasNegativasMes = TimeSpan.Zero;

            int totalHorasEsperadasMes = diasUteisNoMes * horasEsperadasDeTrabalhoDia;
            if (horasTrabalhadasMes < totalHorasEsperadasMes)
                totalHorasNegativasMes += TimeSpan.FromHours(totalHorasEsperadasMes - horasTrabalhadasMes);

            return (decimal)totalHorasNegativasMes.TotalHours;
        }

        public decimal RetornaHorasExtrasMes(int diasUteisNoMes, int horasEsperadasDeTrabalhoDia, int horasTrabalhadasMes)
        {
            TimeSpan totalHorasExtrasMes = TimeSpan.Zero;

            int totalHorasEsperadasMes = diasUteisNoMes * horasEsperadasDeTrabalhoDia;
            if (horasTrabalhadasMes > totalHorasEsperadasMes)
                totalHorasExtrasMes += TimeSpan.FromHours(horasTrabalhadasMes - totalHorasEsperadasMes);

            return (decimal)totalHorasExtrasMes.TotalHours;
        }

        public int RetornaQuantidadeDeDiasFaltantesMes(int quantidadeDiasUteisMes, int quantidadeDiasTrabalhadosMes)
        {

            if (quantidadeDiasTrabalhadosMes < quantidadeDiasUteisMes)
                return quantidadeDiasUteisMes - quantidadeDiasTrabalhadosMes;
            return 0;
        }

        public int RetornaQuantidadeDiasExtrasMes(int quantidadeDiasUteisMes, int quantidadeDiasTrabalhadosMes)
        {
            if (quantidadeDiasTrabalhadosMes > quantidadeDiasUteisMes)
                return quantidadeDiasTrabalhadosMes - quantidadeDiasUteisMes;
            return 0;
        }

        public decimal RetornaTotalAReceber(decimal valorHora, int quantidadeHorasTrabalhadasDia) =>
            (decimal)(valorHora * quantidadeHorasTrabalhadasDia);

        public int RetornaQuantidadeTotalHorasTrabalhadasFuncionario(List<RegistroPontoDto> registrosDoFuncionario)
        {
            int totalHoras = 0;
            foreach (var registroFuncionario in registrosDoFuncionario)
            {
                totalHoras += Convert.ToInt32((registroFuncionario.HoraSaida.TotalHours - registroFuncionario.HoraEntrada.TotalHours) - (registroFuncionario.HoraSaidaAlmoco.TotalHours - registroFuncionario.HoraEntradaAlmoco.TotalHours));
            }
            return totalHoras;
        }

        public int RetornaQuantidadeHorasTrabalhadasDia(RegistroPontoDto registroPontoDto) =>
            Convert.ToInt32((registroPontoDto.HoraSaida.TotalHours - registroPontoDto.HoraEntrada.TotalHours) - (registroPontoDto.HoraSaidaAlmoco.TotalHours - registroPontoDto.HoraEntradaAlmoco.TotalHours));


    }
}
