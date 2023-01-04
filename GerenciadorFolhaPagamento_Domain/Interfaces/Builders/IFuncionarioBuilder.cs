using GerenciadorFolhaPagamento_Domain.Builders;
using GerenciadorFolhaPagamento_Domain.Entities;
using System.Collections.Generic;


namespace GerenciadorFolhaPagamento_Domain.Interfaces.Builders
{
    public interface IFuncionarioBuilder
    {
        FuncionarioBuilder VerificaSeFuncionarioJaExiste(int codigoFuncionario, IList<int> codigoJaExistentes, string nome, decimal valorHora, int idDepartamento);
        Funcionario Build();
    }
}
