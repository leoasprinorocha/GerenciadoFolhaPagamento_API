using Dapper;
using GerenciadorFolhaPagamento_Domain.Dtos;
using GerenciadorFolhaPagamento_Domain.Entities;
using GerenciadorFolhaPagamento_Domain.Interfaces.Repositories;
using GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GerenciadorFolhaPagamento_Data.Repositories
{
    public class ParametrosRepository : IParametrosRepository
    {
        private readonly DbSession _session;

        public ParametrosRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<List<ParametrosDto>> RetornaTodosOsParametros()
        {
            var transactional = _session.Transaction;
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Parametros", (SqlConnection)_session.Connection, (SqlTransaction)transactional);
            using (var listaParametros = await sqlCommand.ExecuteReaderAsync())
            {
                return HelpersRepository.DataReaderMapToList<ParametrosDto>(listaParametros);
            }
        }

        public async Task SalvarNovoParametro(Parametros novoParametro)
        {
            var parameters = new { NomeParametro = novoParametro.NomeParametro, ValorParametro = novoParametro.ValorParametro };
            string sqlCommand = @"INSERT INTO Parametros(NomeParametro, ValorParametro) VALUES(@NomeParametro, @ValorParametro)";
            await _session.Connection.ExecuteAsync(sqlCommand, parameters, _session.Transaction);
        }
    }
}
