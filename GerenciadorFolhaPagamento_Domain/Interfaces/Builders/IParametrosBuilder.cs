using GerenciadorFolhaPagamento_Domain.Builders;
using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Builders
{
    public interface IParametrosBuilder
    {
        ParametrosBuilder VerificaSeParametroJaExiste(IList<string> listaParametros, NovoParametroDto novoParametro);
        Parametros Build();
    }
}
