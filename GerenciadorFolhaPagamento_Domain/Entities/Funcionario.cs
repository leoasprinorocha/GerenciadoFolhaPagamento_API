

using System.Collections.Generic;

namespace GerenciadorFolhaPagamento_Domain.Entities
{
    public class Funcionario
    {
        public Funcionario(int codigoFuncionario, IList<int> codigosJaExistentes)
        {
            VerificaSeFuncionarioJaExiste(codigoFuncionario, codigosJaExistentes);
        }

        public int IdFuncionario { get; set; }

        public int Departamento_idDepartamento { get; set; }

        public string NomeFuncionario { get; set; }

        public decimal? ValorHora { get; set; }

        private bool VerificaSeFuncionarioJaExiste(int codigoFuncionario, IList<int> codigoJaExistentes)
        {
            if (codigoJaExistentes.Contains(codigoFuncionario))
                return true;
            return false;

        }
    }
}
