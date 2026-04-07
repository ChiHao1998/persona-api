using System.ComponentModel;
using System.Data;
using Common.Interface;
using DatabaseBroker.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DatabaseBroker.Service
{
    public class UnitOfWorkBrokerService<T>(T dataContext) : IUnitOfWorkBrokerService<T>, IScopedService
        where T : DbContext
    {
        public async Task BeginTransactionAsync(
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            IDbContextTransaction? transaction = dataContext.Database.CurrentTransaction;

            if (transaction is not null)
                return;

            await dataContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            IDbContextTransaction transaction = dataContext.Database.CurrentTransaction ??
            throw new InvalidAsynchronousStateException($"No active transaction to commit.");

            await SaveChangesAsync(cancellationToken);

            await dataContext.Database.CommitTransactionAsync(cancellationToken);

            await transaction.DisposeAsync();
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            IDbContextTransaction? transaction = dataContext.Database.CurrentTransaction;

            if (transaction is null)
                return;

            await dataContext.Database.RollbackTransactionAsync(cancellationToken);

            await transaction.DisposeAsync();

        }
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await dataContext.SaveChangesAsync(cancellationToken);
    }
}