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
	public IOrderRepository Order { get; private set; }
	public IPaymentRepository Payment { get; private set; }
	public UnitOfWork(AppDbContext db, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
	{
		_db = db;
		Product = new ProductRepository(_db);
		Category = new CategoryRepository(_db);
		Auth = new AuthRepository(_db, configuration, httpContextAccessor);
		Cart = new CartRepository(_db, Auth);
		Order = new OrderRepository(_db, Auth);
		Payment = new PaymentRepository(Cart, Auth, Order);
	}
	public async Task Save() => await _db.SaveChangesAsync();
}
