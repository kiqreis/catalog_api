using WebApplication1.Models;
using X.PagedList;

namespace WebApplication1.Repositories;

public interface IProductRepository : IRepository<Product>
{
  Task<IPagedList<Product>> GetProductsAsync(ProductsParameters productsParams);  
} 