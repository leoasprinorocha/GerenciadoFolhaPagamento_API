using GerenciadorFolhaPagamento_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Repositories
{
    public interface IProcessamentoFolhaFuncionarioRepository
    {
        Task SalvaProcessamentoFolhaFuncionario(ProcessamentoFolha_Funcionario processamentoFolha_Funcionario);
    }
}
