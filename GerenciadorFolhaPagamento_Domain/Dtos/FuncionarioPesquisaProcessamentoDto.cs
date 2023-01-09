

using System.Text.Json.Serialization;

namespace GerenciadorFolhaPagamento_Domain.Dtos
{
    public class FuncionarioPesquisaProcessamentoDto
    {
        public string Nome{ get; set; }
        public int Codigo{ get; set; }
        public decimal TotalReceber{ get; set; }
        public double HorasExtras{ get; set; }
        public double HorasDebito{ get; set; }
        public int DiasFalta{ get; set; }
        public int DiasExtras{ get; set; }
        public int DiasTrabalhados{ get; set; }

        [JsonIgnore]
        public int IdProcessamentoFolha{ get; set; }

        
    }
}
