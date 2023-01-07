

using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using GerenciadorFolhaPagamento_Domain.Interfaces.Builders;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Application.Applications
{
    public class ParametroApplication : IParametroApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IParametrosRepository _parametrosRepository;
        private readonly IParametrosBuilder _parametrosBuilder;
        private List<ParametrosDto> _listaParametrosCarregados;

        public ParametroApplication(IUnitOfWork unitOfWork, IParametrosRepository parametrosRepository, IParametrosBuilder parametrosBuilder)
        {
            _unitOfWork = unitOfWork;
            _parametrosRepository = parametrosRepository;
            _parametrosBuilder = parametrosBuilder;
            _listaParametrosCarregados = new List<ParametrosDto>();
        }

        public string RetornaValorParametro(int codigoParametro) =>
            _listaParametrosCarregados.FirstOrDefault(c => c.IdParametro == codigoParametro).ValorParametro;


        public async Task SalvaNovoParametro(List<NovoParametroDto> novosParametrosDto)
        {
            _unitOfWork.BeginTransaction();
            await CarregaParametros();
            foreach (var parametro in novosParametrosDto)
            {
                List<string> listaParametrosJaExistentes = _listaParametrosCarregados.Select(c => c.NomeParametro.ToUpper()).ToList();
                Parametros novoParametroASerCadastrado = _parametrosBuilder.VerificaSeParametroJaExiste(listaParametrosJaExistentes, parametro)
                                                         .Build();

                if (novoParametroASerCadastrado != null)
                {
                    await _parametrosRepository.SalvarNovoParametro(novoParametroASerCadastrado);
                    _unitOfWork.Commit();
                }
            }
        }

        private async Task CarregaParametros()
        {
            try
            {
                if (_listaParametrosCarregados.Count == 0)
                {
                    _listaParametrosCarregados = await _parametrosRepository.RetornaTodosOsParametros();
                }
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }
}
