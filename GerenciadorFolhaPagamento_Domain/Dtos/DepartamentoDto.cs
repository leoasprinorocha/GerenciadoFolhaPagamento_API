using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Dtos
{
    public class DepartamentoDto
    {

        public int IdDepartamento { get; set; }

        public string NomeDepartamento { get; set; }
        public string MesVigencia { get; set; }
        public string AnoVigencia{ get; set; }
    }
}
