using BlazorEcommerce.Server.Data;

namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public class ProductRatingsRepository : Repository<ProductRatings>, IProductRatingsRepository
{
	public ProductRatingsRepository(AppDbContext db) : base(db)
	{
	}
}
