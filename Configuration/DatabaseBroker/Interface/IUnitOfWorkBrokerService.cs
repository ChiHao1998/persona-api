using System.Data;
using Microsoft.EntityFrameworkCore;

namespace DatabaseBroker.Interface
{
    public interface IUnitOfWorkBrokerService<T> where T : DbContext
    {
        Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}