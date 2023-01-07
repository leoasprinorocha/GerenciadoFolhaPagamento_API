using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using GerenciadorFolhaPagamento_Domain.Interfaces.Builders;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace GerenciadorFolhaPagamento_Application.Applications
{
    public class FuncionarioApplication : IFuncionarioApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IFuncionarioBuilder _funcionarioBuilder;

        public FuncionarioApplication(IUnitOfWork unitOfWork, IFuncionarioRepository funcionarioRepository, IFuncionarioBuilder funcionarioBuilder)
        {
            _unitOfWork = unitOfWork;
            _funcionarioRepository = funcionarioRepository;
            _funcionarioBuilder = funcionarioBuilder;
        }

        public async Task<List<FuncionarioDto>> RecuperaTodosFuncionarios()
        {
            try
            {
                return await _funcionarioRepository.RecuperaTodosFuncionarios();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        public async Task SalvarFuncionario(NovoFuncionarioDto novoFuncionario)
        {
            _unitOfWork.BeginTransaction();

            List<int> listaFuncionariosJaExistentes = await _funcionarioRepository.RecuperaOsCodigosDeTodosOsFuncionarios();
            Funcionario novoFuncionarioASerCadastrado = _funcionarioBuilder.VerificaSeFuncionarioJaExiste(novoFuncionario.IdFuncionario, listaFuncionariosJaExistentes, novoFuncionario.NomeFuncionario, novoFuncionario.ValorHora, novoFuncionario.IdDepartamento)
                                            .Build();

            if (novoFuncionarioASerCadastrado != null)
            {
                await _funcionarioRepository.SalvaNovoFuncionario(novoFuncionarioASerCadastrado);
                _unitOfWork.Commit();
            }
        }
    }
}
