using System.Linq.Expressions;

namespace WebApplication1.Repositories;

public interface IRepository<T>
{
  Task<IEnumerable<T>> GetAllAsync();
  Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
  T Create(T entity);
  T Update(T entity);
  void Delete(int id);
} 