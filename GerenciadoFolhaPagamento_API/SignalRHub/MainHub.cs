using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GerenciadoFolhaPagamento_API.SignalRHub
{
    public class MainHub : Hub
    {
        private readonly IProcessamentoFolhaApplication _processamentoFolhaApplication;

        public MainHub(IProcessamentoFolhaApplication processamentoFolhaApplication)
        {
            _processamentoFolhaApplication= processamentoFolhaApplication;
        }
        public async IAsyncEnumerable<List<PesquisaDepartamentosProcessadosDto>> RetornaProcessamentos(CancellationToken cancellationToken)
        {
            yield return await _processamentoFolhaApplication.RetornaTodosOsProcessamentos();
            await Task.Delay(1000, cancellationToken);

        }
    }
}
