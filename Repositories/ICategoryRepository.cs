using WebApplication1.Models;
using X.PagedList;

namespace WebApplication1.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
  Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters productsParams);
}