using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Applications;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Application.Applications
{
    public class ProcessamentoFolhaFuncionarioApplication : IProcessamentoFolhaFuncionarioApplication
    {
        private readonly IProcessamentoFolhaFuncionarioRepository _processamentoFolhaFuncionarioRepository;
        public ProcessamentoFolhaFuncionarioApplication(IProcessamentoFolhaFuncionarioRepository processamentoFolhaFuncionarioRepository)
        {
            _processamentoFolhaFuncionarioRepository = processamentoFolhaFuncionarioRepository;
        }

        public async Task SalvaProcessamentoFolhaFuncionario(ProcessamentoFolha_Funcionario processamentoFolha_Funcionario) =>
        await _processamentoFolhaFuncionarioRepository.SalvaProcessamentoFolhaFuncionario(processamentoFolha_Funcionario);

    }
}
