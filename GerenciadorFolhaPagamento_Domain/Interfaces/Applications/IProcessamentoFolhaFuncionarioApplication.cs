

using GerenciadorFolhaPagamento_Domain.Entities;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Applications
{
    public interface IProcessamentoFolhaFuncionarioApplication
    {
        Task SalvaProcessamentoFolhaFuncionario(ProcessamentoFolha_Funcionario processamentoFolha_Funcionario);
    }
}
