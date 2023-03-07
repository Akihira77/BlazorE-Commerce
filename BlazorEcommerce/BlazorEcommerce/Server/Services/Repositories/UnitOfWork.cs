using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly AppDbContext _db;
	public IProductRepository Product { get; private set; }
	public ICategoryRepository Category { get; private set; }
	public ICartRepository Cart { get; private set; }
	public IAuthRepository Auth { get; private set; }
	public UnitOfWork(AppDbContext db, IConfiguration configuration)
	{
		_db = db;
		Product = new ProductRepository(_db);
		Category = new CategoryRepository(_db);
		Cart = new CartRepository(_db);
		Auth = new AuthRepository(_db, configuration);
	}
	public async Task Save()
	{
		await _db.SaveChangesAsync();
	}
}
