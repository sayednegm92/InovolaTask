using InovolaTask.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InovolaTask.Application.BaseRepository;

public class RepositoryApp<T> : IRepositoryApp<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected DbSet<T> Entity = null;

    public RepositoryApp(ApplicationDbContext context)
    {
        _context = context;
        Entity = _context.Set<T>();
    }
    public async Task<T> GetByIdAsync(int id)
    {
        return await Entity.FindAsync(id);

    }
    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereCondition)
    {
        return await Entity.Where(whereCondition).FirstOrDefaultAsync();
    }
    public virtual async Task<T> AddAsync(T entity)
    {
        await Entity.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }
    public virtual async Task<T> UpdateAsync(T entity)
    {
        Entity.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    public virtual async Task DeleteAsync(T entity)
    {
        Entity.Remove(entity);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await Entity.AnyAsync(predicate);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Entity.ToListAsync();
    }
    public virtual IEnumerable<T> Seach(Func<T, string> propertySelector, string searchTerm)
    {
        return Entity.AsEnumerable().Where(prop => propertySelector(prop).Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
    }

}
