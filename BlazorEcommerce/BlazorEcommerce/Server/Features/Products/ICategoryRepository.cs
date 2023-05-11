using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Products;

public interface ICategoryRepository : IRepository<Category>
{
    void Update(Category obj);
}