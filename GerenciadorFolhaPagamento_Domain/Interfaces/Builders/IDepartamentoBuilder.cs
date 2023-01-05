using GerenciadorFolhaPagamento_Domain.Builders;
using GerenciadorFolhaPagamento_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Builders
{
    public interface IDepartamentoBuilder
    {
        DepartamentoBuilder VerificaSeDepartamentoJaExiste(string nomeDepartamento, IList<string> departamentosJaExistentes);
        Departamento Build();
    }
}
