
using System;

namespace GerenciadorFolhaPagamento_Infrastructure.DbSessionManagerConfig
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
