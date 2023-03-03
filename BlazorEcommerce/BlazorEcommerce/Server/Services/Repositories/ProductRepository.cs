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

    public async Task<Product> GetProduct(int id)
    {
        return await _db.Products
                .Include(p => p.Variants)
                .ThenInclude(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(string? categoryUrl = null)
    {
		return await _db.Products
				.Where(p => p.Category.Url == categoryUrl)
				.Include(p => p.Variants)
				.ThenInclude(p => p.ProductType)
				.ToListAsync();
    }

    public void Update(Product obj)
	{
		_db.Products.Update(obj);
	}
}
