using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Builders
{
    public class DepartamentoBuilder : IDepartamentoBuilder
    {
        private Departamento _departamento;

        public DepartamentoBuilder()
        {
            _departamento = new Departamento();
        }

        public DepartamentoBuilder VerificaSeDepartamentoJaExiste(string nomeDepartamento, IList<string> departamentosJaExistentes)
        {

            if (departamentosJaExistentes.Contains(nomeDepartamento.ToUpper()))
            {
                _departamento = null;
                return this;
            }
            else
                _departamento.NomeDepartamento = nomeDepartamento;

            return this;
        }

        public Departamento Build() =>
        _departamento;
    }
}
