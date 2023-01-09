using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Repositories
{
    public interface IProcessamentoFolhaRepository
    {
        Task<int> SalvaProcessamentoFolha(ProcessamentoFolha processamentoFolha);
        Task<List<PesquisaDepartamentosProcessadosDto>> PesquisaDepartamentosProcessados();
        void LimparDadosProcessados();
    }
}
