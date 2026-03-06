using Messenger.Domain.Entities;

using Microsoft.EntityFrameworkCore.Storage;

namespace Messenger.Domain.Interfaces;

public interface IBaseRepository<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    Task CreateAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task<IReadOnlyList<TEntity>> GetAsync(TFilter? filter = null);
    
    Task<IDbContextTransaction> BeginTransactionAsync();
}