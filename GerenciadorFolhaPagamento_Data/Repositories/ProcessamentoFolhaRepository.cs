

using Dapper;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
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

        public async Task<int> SalvaProcessamentoFolha(ProcessamentoFolha processamentoFolha)
        {
            var parameters = new
            {
                idDepartamento = processamentoFolha.Departamento_idDepartamento,
                mesVigencia = processamentoFolha.MesVigencia,
                totalPagamentos = processamentoFolha.TotalPagamentos,
                totalDescontos = processamentoFolha.TotalDescontos,
                totalExtras = processamentoFolha.TotalExtras
            };

            string sqlCommand = @"DECLARE @result INT;
                                 EXEC @result = sp_GravaProcessamentoFolha @idDepartamento, @mesVigencia, @totalPagamentos, @totalDescontos, @totalExtras;
                                 SELECT @result;";

            var result = await _session.Connection.ExecuteScalarAsync(sqlCommand, parameters, _session.Transaction);
            return (int)result;
        }
    }
}
