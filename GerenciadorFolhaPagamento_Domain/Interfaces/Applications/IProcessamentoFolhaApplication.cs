using GerenciadorFolhaPagamento_Domain.Dtos;
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
        void IniciaProcessamento();
        Task<int> SalvaProcessamentoFolha(ProcessamentoFolha processamentoFolha);
        Task<List<string>> RetornaArquivosQueEstaoNaPastaDeProcessamento();
        Task<List<PesquisaDepartamentosProcessadosDto>> RetornaTodosOsProcessamentos();
        void LimparDadosProcessados();

    }
}
