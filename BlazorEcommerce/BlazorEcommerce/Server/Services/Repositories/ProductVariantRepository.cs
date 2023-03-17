using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

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
