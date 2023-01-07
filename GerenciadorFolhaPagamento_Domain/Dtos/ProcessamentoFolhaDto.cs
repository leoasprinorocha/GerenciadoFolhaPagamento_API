

namespace GerenciadorFolhaPagamento_Domain.Dtos
{
    public class ProcessamentoFolhaDto
    {
        public decimal TotalPagamentos { get; set; }
        public int TotalHorasTrabalhadas { get; set; }
        public decimal TotalDescontos { get; set; }
        public int TotalHorasExtras { get; set; }
        public int TotalHorasNegativas { get; set; }
        public int TotalHorasEsperadasMes { get; set; }
    }
}
