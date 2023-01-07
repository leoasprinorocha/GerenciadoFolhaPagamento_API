using GerenciadorFolhaPagamento_Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Applications
{
    public interface IParametroApplication
    {
        Task SalvaNovoParametro(List<NovoParametroDto> novoParametroDto);
        string RetornaValorParametro(int codigoParametro);
    }
}
