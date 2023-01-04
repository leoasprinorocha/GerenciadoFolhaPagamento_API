
using GerenciadorFolhaPagamento_Domain.Dtos;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Applications
{
    public interface IFuncionarioApplication
    {
        Task SalvarFuncionario(NovoFuncionarioDto novoFuncionario);
    }
}
