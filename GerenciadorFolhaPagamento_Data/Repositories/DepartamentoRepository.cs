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
            var transactional = _session.Transaction;
            List<string> listaDepartamentos = new List<string>();
            SqlCommand sqlCommand = new SqlCommand("SELECT NomeDepartamento FROM Departamento", (SqlConnection)_session.Connection, (SqlTransaction)transactional);
            using (var nomesDepartamentos = await sqlCommand.ExecuteReaderAsync())
            {
                while(nomesDepartamentos.Read()){
                    listaDepartamentos.Add(nomesDepartamentos.GetString(0).ToUpper());
                }
            }

            return listaDepartamentos;

        }

        public async Task<List<DepartamentoDto>> RecuperaTodosOsDepartamentos()
        {
            var transactional = _session.Transaction;
            List<DepartamentoDto> listaDepartamentos = new List<DepartamentoDto>();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Departamento", (SqlConnection)_session.Connection, (SqlTransaction)transactional);
            
            using (var departamento = await sqlCommand.ExecuteReaderAsync()){
                while (departamento.Read())
                {
                    listaDepartamentos.Add(departamento.ConvertToObject<DepartamentoDto>());
                }
            }

            return listaDepartamentos;
                
        }

        public async Task<int> RetornaIdDepartamentoPeloNome(string nome)
        {
            var transactional = _session.Transaction;
            SqlCommand sqlCommand = new SqlCommand("SELECT IdDepartamento FROM Parametros WHERE NomeDepartamento = @nomeDepartamento", (SqlConnection)_session.Connection, (SqlTransaction)transactional);
            sqlCommand.Parameters.AddWithValue("@nomeDepartamento", nome);
            using (var idParametro = await sqlCommand.ExecuteReaderAsync())
            {
                return idParametro.GetInt32(0);
            }
        }

        public async Task<int> SalvaNovoDepartamento(Departamento novoDepartamento)
        {
            var parameters = new { nomeDepartamento = novoDepartamento.NomeDepartamento };
            string sqlCommand = @"DECLARE @result INT;
                                EXEC @result = sp_GravaDepartamento @nomeDepartamento;
                                SELECT @result;";
            var result = await _session.Connection.ExecuteScalarAsync<int>(sqlCommand, parameters, _session.Transaction);
            return (int)result;
        }
    }
}
