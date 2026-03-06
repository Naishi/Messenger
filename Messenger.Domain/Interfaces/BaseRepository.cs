using Messenger.Domain.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Messenger.Domain.Interfaces;

public abstract class BaseRepository<TEntity, TFilter> : IBaseRepository<TEntity, TFilter>
    where TEntity : class
    where TFilter : class
{
    protected readonly DataContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(DataContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<IReadOnlyList<TEntity>> GetAsync(TFilter? filter = null)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = ApplyFilter(query, filter);
        }

        return await query.ToListAsync();
    }

    protected abstract IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, TFilter filter);

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}