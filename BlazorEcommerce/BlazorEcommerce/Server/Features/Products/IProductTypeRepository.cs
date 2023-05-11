using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Products;

public interface IProductTypeRepository : IRepository<ProductType>
{
    void Update(ProductType productType);
}