namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IUnitOfWork
{
	IProductRepository Product { get; }
	ICategoryRepository Category { get; }
	ICartRepository Cart { get; }
	IAuthRepository Auth { get; }
	Task Save();
}
