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
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly DbSession _session;

        public FuncionarioRepository(DbSession session)
        {
            _session = session;
        }

        public async Task<List<int>> RecuperaOsCodigosDeTodosOsFuncionarios()
        {
            var transactional = _session.Transaction;
            SqlCommand sqlCommand = new SqlCommand("SELECT CodigoRegistroFuncionario FROM Funcionario", (SqlConnection)_session.Connection, (SqlTransaction)transactional);
            List<int> listaCodigos = new List<int>();
            using (var codigos = await sqlCommand.ExecuteReaderAsync())
            {
                while (codigos.Read())
                {
                    listaCodigos.Add((int)codigos.GetValue(codigos.GetOrdinal("CodigoRegistroFuncionario")));
                }


            }

            return listaCodigos;
        }

        public async Task<List<FuncionarioDto>> RecuperaTodosFuncionarios()
        {
            var transactional = _session.Connection.BeginTransaction();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Funcionario", (SqlConnection)_session.Connection, (SqlTransaction)transactional);
            using (var listaFuncionarios = await sqlCommand.ExecuteReaderAsync())
            {
                return HelpersRepository.DataReaderMapToList<FuncionarioDto>(listaFuncionarios);
            }
        }

        public async Task SalvaNovoFuncionario(Funcionario novoFuncionario)
        {
            var parameters = new
            {
                IdDepartamento = novoFuncionario.Departamento_idDepartamento,
                NomeFuncionario = novoFuncionario.NomeFuncionario,
                ValorHora = novoFuncionario.ValorHora,
                CodigoRegistroFuncionario = novoFuncionario.CodigoRegistroFuncionario
            };
            string sqlCommand = @"INSERT INTO Funcionario(Departamento_idDepartamento, NomeFuncionario, ValorHora, CodigoRegistroFuncionario) 
                                 VALUES(@IdDepartamento, @NomeFuncionario, @ValorHora, @CodigoRegistroFuncionario)";

            await _session.Connection.ExecuteAsync(sqlCommand, parameters, _session.Transaction);
        }

    }
}
