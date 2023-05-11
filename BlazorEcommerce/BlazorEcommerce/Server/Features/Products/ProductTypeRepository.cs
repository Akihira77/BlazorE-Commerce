using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Features.Base;

namespace BlazorEcommerce.Server.Features.Products;

public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
{
    private readonly AppDbContext _db;

    public ProductTypeRepository(AppDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(ProductType productType)
    {
        _db.ProductTypes.Update(productType);
    }
}