

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GerenciadorFolhaPagamento_Domain.Dtos
{
    public class PesquisaDepartamentosProcessadosDto
    {
        public string Departamento { get; set; }
        public string MesVigencia { get; set; }
        public string AnoVigencia { get; set; }
        public decimal TotalPagar { get; set; }
        public decimal TotalDescontos { get; set; }
        public decimal TotalExtras { get; set; }

        [JsonIgnore]
        public int IdProcessamentoFolha { get; set; }
        public List<FuncionarioPesquisaProcessamentoDto> Funcionarios { get; set; }

    }


}
