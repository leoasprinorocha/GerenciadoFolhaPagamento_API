using System.Data;
using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig
{
    public class DbSession : IDisposable
    {
        private Guid _id;
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        public IConfiguration Configuration { get; }

        public DbSession(IConfiguration configuration)
        {
            Configuration = configuration;
            _id = Guid.NewGuid();
            Connection = new SqlConnection(Configuration.GetConnectionString("SqlServerDbConnection"));
            Connection.Open();
        }

        public void OpenConnection()
        {
            this.Connection = new SqlConnection(Configuration.GetConnectionString("SqlServerDbConnection"));
            this.Connection.Open();
        }



        public void Dispose() => Connection?.Dispose();
    }
}
