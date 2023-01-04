
using GerenciadorFolhaPagamento_Domain.Enums;
using System;

namespace GerenciadorFolhaPagamento_Domain.Entities
{
    public class ProcessamentoFolha
    {
        public int IdProcessamentoFolha { get; set; }

        public int Departamento_idDepartamento { get; set; }

        public string MesVigencia { get; set; }

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

        public int QuantidadeDeHorasTrabalhadasEsperadaDia { get { return 8; } private set { } }
        public int QuantidadeDeHorasDeAlmocoEsperadaDia{ get { return 1; } private set { } }
    }
}
