using Microsoft.EntityFrameworkCore;

namespace Configuration;

public class DatabaseBroker<T>(T dataContext) where T : DbContext
{
    protected async ValueTask<TEntity> InsertAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await dataContext.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    protected TEntity Delete<TEntity>(TEntity entity)
    where TEntity : class
    {
        dataContext.Set<TEntity>().Remove(entity);
        return entity;
    }

    protected async ValueTask<IEnumerable<TEntity>> InsertRangeAsync<TEntity>(IEnumerable<TEntity> models)
        where TEntity : class
    {
        await dataContext.Set<TEntity>().AddRangeAsync(models);
        return models;
    }

    protected IEnumerable<TEntity> DeleteRange<TEntity>(IEnumerable<TEntity> models)
        where TEntity : class
    {
        dataContext.Set<TEntity>().RemoveRange(models);
        return models;
    }

    protected IQueryable<TEntity> Select<TEntity>(bool asNoTracking = true)
        where TEntity : class
    {
        IQueryable<TEntity> query = dataContext.Set<TEntity>();

        return asNoTracking
            ? query.AsNoTracking()
            : query;
    }
}
