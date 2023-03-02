using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
	private readonly AppDbContext _db;

	public ProductRepository(AppDbContext db) : base(db)
	{
		_db = db;
	}

	public void Update(Product obj)
	{
		_db.Products.Update(obj);
	}
}
