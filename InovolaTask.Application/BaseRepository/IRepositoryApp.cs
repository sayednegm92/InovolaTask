using System.Linq.Expressions;

namespace InovolaTask.Application.BaseRepository;

public interface IRepositoryApp<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> whereCondition);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    IEnumerable<T> Seach(Func<T, string> propertySelector, string searchTerm);

}
