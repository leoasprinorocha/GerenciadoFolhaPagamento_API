
using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Repositories
{
    public interface IFuncionarioRepository
    {
        Task SalvaNovoFuncionario(Funcionario novoFuncionario);
        Task<List<int>> RecuperaOsCodigosDeTodosOsFuncionarios();

        Task<List<FuncionarioDto>> RecuperaTodosFuncionarios();
    }
}
