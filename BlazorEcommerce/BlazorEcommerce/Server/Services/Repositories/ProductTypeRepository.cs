using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

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
