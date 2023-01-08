using GerenciadorFolhaPagamento_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Applications
{
    public interface IProcessamentoFolhaApplication
    {
        Task<bool> IniciaProcessamento();
        Task<int> SalvaProcessamentoFolha(ProcessamentoFolha processamentoFolha);

    }
}
