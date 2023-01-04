
using System;

namespace GerenciadorFolhaPagamento_Domain.Entities
{
    public class RegistroPonto
    {
        public int IdRegistroPonto { get; set; }

        public int Funcionario_Departamento_idDepartamento { get; set; }

        public int Funcionario_idFuncionario { get; set; }

        public DateTime DataRegistro { get; set; }

        public TimeSpan Entrada { get; set; }

        public TimeSpan Saida { get; set; }

        public TimeSpan EntradaAlmoco { get; set; }

        public TimeSpan SaidaAlmoco { get; set; }
    }
}
