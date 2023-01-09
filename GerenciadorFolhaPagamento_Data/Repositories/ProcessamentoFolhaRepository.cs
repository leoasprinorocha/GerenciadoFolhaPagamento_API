

using Dapper;
using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Data.Repositories
{
    public class ProcessamentoFolhaRepository : IProcessamentoFolhaRepository
    {
        private readonly DbSession _session;

        public ProcessamentoFolhaRepository(DbSession session)
        {
            _session = session;
        }

        public void LimparDadosProcessados()
        {
            var transactional = _session.Transaction;
            SqlCommand sqlCommand = new SqlCommand(
                                 @"delete from Funcionario;
                                  delete from Departamento;
                                  delete from ProcessamentoFolha;
                                  delete from ProcessamentoFolha_Funcionario;", (SqlConnection)_session.Connection, (SqlTransaction)transactional);

            sqlCommand.ExecuteNonQuery();
        }

        public async Task<List<PesquisaDepartamentosProcessadosDto>> PesquisaDepartamentosProcessados()
        {
            var transactional = _session.Transaction;
            List<PesquisaDepartamentosProcessadosDto> departamentosProcessados = new List<PesquisaDepartamentosProcessadosDto>();
            List<FuncionarioPesquisaProcessamentoDto> funcionariosProcessados = new List<FuncionarioPesquisaProcessamentoDto>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT 
                                                      dep.NomeDepartamento As Departamento, 
                                                      procFolha.MesVigencia, 
                                                      procFolha.AnoVigencia, 
                                                      procFolha.TotalPagamentos As TotalPagar, 
                                                      procFolha.TotalDescontos,  
                                                      procFolha.IdProcessamentoFolha 
                                                    FROM 
                                                      ProcessamentoFolha procFolha 
                                                      inner join Departamento dep on procFolha.Departamento_idDepartamento = dep.IdDepartamento", (SqlConnection)_session.Connection, (SqlTransaction)transactional);

            using (var departamentos = await sqlCommand.ExecuteReaderAsync())
            {
                while (departamentos.Read())
                {
                    departamentosProcessados.Add(departamentos.ConvertToObject<PesquisaDepartamentosProcessadosDto>());
                }
            }
            var parameters = new string[departamentosProcessados.Count];
            string sqlQuery = @"SELECT   
                                DISTINCT(Total.Nome), 
                                Total.Codigo, 
                                Total.TotalReceber, 
                                Total.HorasExtras, 
                                Total.HorasDebito, 
                                Total.DiasFalta, 
                                Total.DiasExtras, 
                                Total.DiasTrabalhados,
                                Total.IdProcessamentoFolha,
                                Total.ValorHora
                            FROM 
                                (
                                SELECT 
                                    func.NomeFuncionario as Nome, 
                                    func.CodigoRegistroFuncionario As Codigo,
                                    func.ValorHora as ValorHora,
                                    (
                                    SELECT 
                                        SUM(proceFunc.TotalAReceber) 
                                    from 
                                        ProcessamentoFolha_Funcionario as proceFunc 
                                    WHERE 
                                        proceFunc.Funcionario_idFuncionario = procFuncionario.Funcionario_idFuncionario
                                    ) as TotalReceber, 
                                    (
                                    SELECT 
                                        SUM(proceFunc.HorasExtras) 
                                    from 
                                        ProcessamentoFolha_Funcionario as proceFunc 
                                    WHERE 
                                        proceFunc.Funcionario_idFuncionario = procFuncionario.Funcionario_idFuncionario
                                    ) as HorasExtras, 
                                    (
                                    SELECT 
                                        SUM(proceFunc.HorasDebito) 
                                    from 
                                        ProcessamentoFolha_Funcionario as proceFunc 
                                    WHERE 
                                        proceFunc.Funcionario_idFuncionario = procFuncionario.Funcionario_idFuncionario
                                    ) as HorasDebito, 
                                    procFuncionario.DiasFalta, 
                                    procFuncionario.DiasExtras, 
                                    procFuncionario.DiasTrabalhados,
                                    procFuncionario.ProcessamentoFolha_idProcessamentoFolha as IdProcessamentoFolha
                                FROM 
                                    ProcessamentoFolha_Funcionario procFuncionario 
                                    inner join Funcionario func on procFuncionario.Funcionario_idFuncionario = func.CodigoRegistroFuncionario 
                                WHERE 
                                    procFuncionario.ProcessamentoFolha_idProcessamentoFolha in({idProcessamentoFolha})
                                ) as Total";

            SqlCommand sqlCommandSubQuery = new SqlCommand(sqlQuery, (SqlConnection)_session.Connection, (SqlTransaction)transactional);

            sqlCommandSubQuery.AddArrayParameters("idProcessamentoFolha", departamentosProcessados.Select(c => c.IdProcessamentoFolha).ToList());

            using (var funcionarios = await sqlCommandSubQuery.ExecuteReaderAsync())
            {
                while (funcionarios.Read())
                {
                    funcionariosProcessados.Add(funcionarios.ConvertToObject<FuncionarioPesquisaProcessamentoDto>());
                }
            }


            foreach (var departamento in departamentosProcessados)
            {
                departamento.Funcionarios = funcionariosProcessados.Where(c => c.IdProcessamentoFolha == departamento.IdProcessamentoFolha).ToList();
                departamento.TotalExtras = funcionariosProcessados.Where(c => c.IdProcessamentoFolha == departamento.IdProcessamentoFolha).ToList().Sum(x => x.ValorHora * Convert.ToDecimal(x.HorasExtras));
            }


            return departamentosProcessados;
        }

        public async Task<int> SalvaProcessamentoFolha(ProcessamentoFolha processamentoFolha)
        {
            var parameters = new
            {
                idDepartamento = processamentoFolha.Departamento_idDepartamento,
                mesVigencia = processamentoFolha.MesVigencia,
                totalPagamentos = processamentoFolha.TotalPagamentos,
                totalDescontos = processamentoFolha.TotalDescontos,
                totalExtras = processamentoFolha.TotalExtras,
                anoVigencia = processamentoFolha.AnoVigencia
            };

            string sqlCommand = @"DECLARE @result INT;
                                 EXEC @result = sp_GravaProcessamentoFolha @idDepartamento, @mesVigencia, @totalPagamentos, @totalDescontos, @totalExtras, @anoVigencia;
                                 SELECT @result;";

            var result = await _session.Connection.ExecuteScalarAsync(sqlCommand, parameters, _session.Transaction);
            return (int)result;
        }
    }
}
