using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;

namespace WebApplication1.Repositories;

public class Repository<T>(AppDbContext context) : IRepository<T>
  where T : class
{
  public async Task<IEnumerable<T>> GetAllAsync()
  {
    return await context.Set<T>().AsNoTracking().ToListAsync();
  }
  
  public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
  {
    return await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(predicate);
  }

  public T Create(T entity)
  {
    context.Set<T>().Add(entity);
    return entity;
  }

  public T Update(T entity)
  {
    context.Entry(entity).State = EntityState.Modified;
    return entity;
  }

  public void Delete(int id)
  {
    var entity = context.Set<T>().Find(id);
    context.Set<T>().Remove(entity!);
  }
}