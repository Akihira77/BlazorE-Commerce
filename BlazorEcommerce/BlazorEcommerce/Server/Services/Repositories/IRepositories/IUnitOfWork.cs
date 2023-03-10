namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IUnitOfWork
{
	IProductRepository Product { get; }
	ICategoryRepository Category { get; }
	ICartRepository Cart { get; }
	IAuthRepository Auth { get; }
	IOrderRepository Order { get; }
	Task Save();
}
