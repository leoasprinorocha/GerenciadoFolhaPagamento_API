using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Dtos
{
    public class ParametrosDto
    {
        public int IdParametro { get; set; }
        public string NomeParametro { get; set; }
        public string ValorParametro { get; set; }
    }
}
