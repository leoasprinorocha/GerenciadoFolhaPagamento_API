using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Dtos
{
    public class RegistroPontoDto
    {
        public int CodigoFuncionario{ get; set; }
        public string Nome{ get; set; }
        public decimal ValorHora{ get; set; }
        public DateTime DataRegistro{ get; set; }
        public TimeSpan HoraEntrada{ get; set; }
        public TimeSpan HoraSaida{ get; set; }
        public TimeSpan HoraEntradaAlmoco{ get; set; }
        public TimeSpan HoraSaidaAlmoco{ get; set; }

    }
}
