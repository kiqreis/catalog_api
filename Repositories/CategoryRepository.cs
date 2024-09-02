using WebApplication1.Context;
using WebApplication1.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace WebApplication1.Repositories;

public class CategoryRepository(AppDbContext context) : Repository<Category>(context), ICategoryRepository
{
  public async Task<IPagedList<Category>> GetCategoriesAsync(CategoriesParameters categoriesParams)
  {
    var categories = await GetAllAsync();
    var orderedCategories = categories.OrderBy(c => c.Id).AsQueryable();
    var result = orderedCategories.ToPagedList(categoriesParams.PageNumber, categoriesParams.PageSize);

    return result;
  }
}