using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Builders
{
    public class FuncionarioBuilder : IFuncionarioBuilder
    {
        private Funcionario _funcionario;
        public FuncionarioBuilder()
        {
            _funcionario = new Funcionario();
        }

        public FuncionarioBuilder VerificaSeFuncionarioJaExiste(int codigoFuncionario, IList<int> codigoJaExistentes, string nome, decimal valorHora, int idDepartamento)
        {
            if (codigoJaExistentes.Contains(codigoFuncionario))
            {
                _funcionario = null;
                return this;
            }
            else
            {
                PreencheDadosFuncionario(nome, valorHora, idDepartamento);
                return this;
            }


        }

        private void PreencheDadosFuncionario(string nome, decimal valorHora, int idDepartamento)
        {
            _funcionario.NomeFuncionario = nome;
            _funcionario.Departamento_idDepartamento = idDepartamento;
            _funcionario.ValorHora = valorHora;
        }

        public Funcionario Build() =>
        _funcionario;
    }
}
