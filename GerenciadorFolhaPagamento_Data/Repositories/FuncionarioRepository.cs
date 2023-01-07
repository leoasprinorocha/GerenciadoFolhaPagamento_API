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
            string sqlCommand = "SELECT IdFuncionario FROM Funcionario";
            return (List<int>)await _session.Connection.QueryAsync<List<int>>(sqlCommand, _session.Transaction);
        }

        public async Task<List<FuncionarioDto>> RecuperaTodosFuncionarios()
        {
            var transactional = _session.Connection.BeginTransaction();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Funcionario", (SqlConnection)_session.Connection, (SqlTransaction)transactional);
            var listaFuncionarios = await sqlCommand.ExecuteReaderAsync();
            return HelpersRepository.DataReaderMapToList<FuncionarioDto>(listaFuncionarios);
        }

        public async Task SalvaNovoFuncionario(Funcionario novoFuncionario)
        {
            string sqlCommand = @"INSERT INTO Funcionario(Departamento_idDepartamento, NomeFuncionario, ValorHora) 
                                 VALUES(pIdDepartamento, pNomeFuncionario, pValorHora)";

            await _session.Connection.ExecuteAsync(sqlCommand, new
            {
                pIdDepartamento = novoFuncionario.Departamento_idDepartamento,
                pNomeFuncionario = novoFuncionario.NomeFuncionario,
                pValorHora = novoFuncionario.ValorHora
            }, _session.Transaction);
        }

    }
}
