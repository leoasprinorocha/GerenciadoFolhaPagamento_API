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
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly DbSession _session;

        public DepartamentoRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<List<string>> RecuperaOsNomesDeTodosOsDepartamentos()
        {
            string sqlCommand = "SELECT NomeDepartamento FROM Departamento";
            return (List<string>)await _session.Connection.QueryAsync<List<string>>(sqlCommand, _session.Transaction);
        }

        public async Task<List<DepartamentoDto>> RecuperaTodosOsDepartamentos()
        {
            var transactional = _session.Connection.BeginTransaction();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Departamento", (SqlConnection)_session.Connection, (SqlTransaction)transactional);
            var listaDepartamentos = await sqlCommand.ExecuteReaderAsync();
            return HelpersRepository.DataReaderMapToList<DepartamentoDto>(listaDepartamentos);
        }

        public async Task SalvaNovoDepartamento(Departamento novoDepartamento)
        {
            string sqlCommand = @"INSERT INTO Departamento(NomeDepartamento) VALUES(pNomDepartamento)";

            await _session.Connection.ExecuteAsync(sqlCommand, new { pNomeDepartamento = novoDepartamento.NomeDepartamento }, _session.Transaction);
        }
    }
}
