
using GerenciadorFolhaPagamento_Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Applications
{
    public interface IFuncionarioApplication
    {
        Task SalvarFuncionario(NovoFuncionarioDto novoFuncionario);

        Task<List<FuncionarioDto>> RecuperaTodosFuncionarios();
        
    }
}
