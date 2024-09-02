using WebApplication1.Context;

namespace WebApplication1.Repositories;

public class UnityOfWork(AppDbContext context) : IUnitOfWork
{
  private IProductRepository? _productRepository;
  private ICategoryRepository? _categoryRepository;

  public IProductRepository ProductRepository
  {
    get { return _productRepository ??= new ProductRepository(context); }
  }

  public ICategoryRepository CategoryRepository
  {
    get { return _categoryRepository ??= new CategoryRepository(context); }
  }

  public async Task CommitAsync()
  {
    await context.SaveChangesAsync();
  }

  public void Dispose()
  {
    context.Dispose();
  }
}