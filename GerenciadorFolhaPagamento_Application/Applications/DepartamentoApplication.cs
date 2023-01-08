using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using GerenciadorFolhaPagamento_Domain.Interfaces.Builders;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GerenciadorFolhaPagamento_Application.Applications
{
    public class DepartamentoApplication : IDepartamentoApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IDepartamentoBuilder _departamentoBuilder;

        public DepartamentoApplication(IUnitOfWork unitOfWork, IDepartamentoRepository departamentoRepository,
                                       IDepartamentoBuilder departamentoBuilder)
        {
            _unitOfWork = unitOfWork;
            _departamentoRepository = departamentoRepository;
            _departamentoBuilder = departamentoBuilder;

        }

        public async Task<List<DepartamentoDto>> RecuperaTodosDepartamentos()
        {
            try
            {

                return await _departamentoRepository.RecuperaTodosOsDepartamentos();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        public async Task<int> RetornaIdDepartamentoPeloNome(string nome) =>
        await _departamentoRepository.RetornaIdDepartamentoPeloNome(nome);


        public async Task<int> SalvarDepartamento(NovoDepartamentoDto novoDepartamento)
        {


            List<string> listaDepartamentosJaExistentes = await _departamentoRepository.RecuperaOsNomesDeTodosOsDepartamentos();

            Departamento novoDepartamentoASerCadastrado = _departamentoBuilder.VerificaSeDepartamentoJaExiste(novoDepartamento.NomeDepartamento, listaDepartamentosJaExistentes)
                                                          .Build();

            if (novoDepartamentoASerCadastrado != null)
            {
                return await _departamentoRepository.SalvaNovoDepartamento(novoDepartamentoASerCadastrado);
            }
            else
            {
                var departamentos = await RecuperaTodosDepartamentos();
                return departamentos.First(c => c.NomeDepartamento.Equals(novoDepartamento.NomeDepartamento)).IdDepartamento;
            }

        }
    }
}
