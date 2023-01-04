

using System.Collections.Generic;

namespace GerenciadorFolhaPagamento_Domain.Entities
{
    public class Departamento
    {
        public int IdDepartamento { get; set; }

        public string NomeDepartamento { get; set; }

        public bool VerificaSeDepartamentoJaExiste(string nomeDepartamento, IList<string> nomesJaExistentes)
        {
            if (nomesJaExistentes.Contains(nomeDepartamento))
                return true;
            return false;
        }
    }
}
