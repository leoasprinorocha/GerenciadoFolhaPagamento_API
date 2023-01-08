using Dapper;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Data.Repositories
{
    public class ProcessamentoFolhaFuncionarioRepository : IProcessamentoFolhaFuncionarioRepository
    {
        private readonly DbSession _session;

        public ProcessamentoFolhaFuncionarioRepository(DbSession session)
        {
            _session = session;
        }
        public async Task SalvaProcessamentoFolhaFuncionario(ProcessamentoFolha_Funcionario processamentoFolha_Funcionario)
        {
            var parameters = new
            {
                idDepartamento = processamentoFolha_Funcionario.Funcionario_Departamento_idDepartamento,
                idFuncionario = processamentoFolha_Funcionario.Funcionario_idFuncionario,
                idProcessamentoFolha = processamentoFolha_Funcionario.IdProcessamentoFolhaFuncionario,
                totalReceber = processamentoFolha_Funcionario.TotalAReceber,
                horasExtras = processamentoFolha_Funcionario.HorasExtras,
                horasDebito = processamentoFolha_Funcionario.HorasDebito,
                diasFalta = processamentoFolha_Funcionario.DiasFalta,
                diasExtras = processamentoFolha_Funcionario.DiasExtras,
                diasTrabalhados = processamentoFolha_Funcionario.DiasTrabalhados
            };

            string sqlCommand = @"INSERT INTO ProcessamentoFolha_Funcionario
                                   (Funcionario_Departamento_idDepartamento
                                   ,Funcionario_idFuncionario
                                   ,ProcessamentoFolha_idProcessamentoFolha
                                   ,TotalAReceber
                                   ,HorasExtras
                                   ,HorasDebito
                                   ,DiasFalta
                                   ,DiasExtras
                                   ,DiasTrabalhados)
                                 VALUES
                                    (@idDepartamento,
                                     @idFuncionario,
                                     @idProcessamentoFolha,
                                     @totalReceber,
                                     @horasExtras,
                                     @horasDebito,
                                     @diasFalta,
                                     @diasExtras,
                                     @diasTrabalhados)";

            await _session.Connection.ExecuteScalarAsync(sqlCommand, parameters, _session.Transaction);
        }
    }
}
