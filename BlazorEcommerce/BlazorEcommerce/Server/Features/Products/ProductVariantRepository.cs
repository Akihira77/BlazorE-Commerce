using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Products;

public class ProductVariantRepository : Repository<ProductVariant>, IProductVariantRepository
{
    private readonly AppDbContext _db;

    public ProductVariantRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(ProductVariant productVariant)
    {
        _db.ProductVariants.Update(productVariant);
    }
}