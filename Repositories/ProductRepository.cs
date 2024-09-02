using WebApplication1.Context;
using WebApplication1.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace WebApplication1.Repositories;

public class ProductRepository(AppDbContext context) : Repository<Product>(context), IProductRepository
{
  public async Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams)
  {
    var products = await GetAllAsync();
    var orderedProducts = products.OrderBy(p => p.Id).AsQueryable();
    var result = orderedProducts.ToPagedList(productsParams.PageNumber, productsParams.PageSize);

    return result;
  }
}