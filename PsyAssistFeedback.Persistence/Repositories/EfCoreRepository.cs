using Microsoft.EntityFrameworkCore;
using PsyAssistFeedback.Application.Interfaces.Repository;
using PsyAssistFeedback.Domain;
using System.Linq.Expressions;

namespace PsyAssistFeedback.Persistence.Repositories;

public class EfCoreRepository<T> : IRepository<T>
    where T : BaseEntity
{
    private readonly PsyAssistContext _context;
    private readonly DbSet<T> _dbSet;

    public EfCoreRepository(PsyAssistContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.FindAsync<T>(id, cancellationToken);
        if (entity is not null)
            _dbSet.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}