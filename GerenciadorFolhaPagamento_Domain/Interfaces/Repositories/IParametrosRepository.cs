using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Domain.Interfaces.Repositories
{
    public interface IParametrosRepository
    {
        Task SalvarNovoParametro(Parametros paramtro);

        Task AtualizaValorParametro(Parametros parametro);
        Task<List<ParametrosDto>> RetornaTodosOsParametros();
        Task<object> RetornaValorParametro(int idParametro);
    }
}
