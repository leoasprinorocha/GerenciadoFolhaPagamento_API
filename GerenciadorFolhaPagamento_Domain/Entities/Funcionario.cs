

using System.Collections.Generic;

namespace GerenciadorFolhaPagamento_Domain.Entities
{
    public class Funcionario
    {
        public Funcionario()
        {
            
        }

        public int IdFuncionario { get; set; }

        public int Departamento_idDepartamento { get; set; }

        public string NomeFuncionario { get; set; }

        public decimal? ValorHora { get; set; }
        
    }
}
