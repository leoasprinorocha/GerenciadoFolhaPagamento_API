

using System;

namespace GerenciadorFolhaPagamento_Domain.Entities
{
    public class ProcessamentoFolha_Funcionario
    {
        public int IdProcessamentoFolhaFuncionario { get; set; }

        public int Funcionario_Departamento_idDepartamento { get; set; }

        public int Funcionario_idFuncionario { get; set; }

        public int ProcessamentoFolha_idProcessamentoFolha { get; set; }

        public decimal? TotalAReceber { get; set; }

        public TimeSpan? HorasExtras { get; set; }

        public TimeSpan? HorasDebito { get; set; }

        public int? DiasFalta { get; set; }

        public int? DiasExtras { get; set; }

        public int? DiasTrabalhados { get; set; }

        public int RetornasHorasNegativasMes(int diasUteisNoMes, int horasEsperadasDeTrabalhoDia, int horasTrabalhadasMes)
        {

            int totalHorasEsperadasMes = diasUteisNoMes * horasEsperadasDeTrabalhoDia;
            if (horasTrabalhadasMes < totalHorasEsperadasMes)
                return totalHorasEsperadasMes - horasTrabalhadasMes;
            else
                return 0;

        }

        public int RetornaHorasExtrasMes(int diasUteisNoMes, int horasEsperadasDeTrabalhoDia, int horasTrabalhadasMes)
        {
            int totalHorasEsperadasMes = diasUteisNoMes * horasEsperadasDeTrabalhoDia;
            if (horasTrabalhadasMes > totalHorasEsperadasMes)
                return horasTrabalhadasMes - totalHorasEsperadasMes;
            return 0;
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

        public decimal RetornaTotalAReceber(decimal valorHora, int quantidadeHorasTrabalhadas) =>
            (decimal)(valorHora * quantidadeHorasTrabalhadas);

    }
}
