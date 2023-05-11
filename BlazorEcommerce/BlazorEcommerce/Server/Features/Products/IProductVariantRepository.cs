using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Products;

public interface IProductVariantRepository : IRepository<ProductVariant>
{
    void Update(ProductVariant productVariant);
}