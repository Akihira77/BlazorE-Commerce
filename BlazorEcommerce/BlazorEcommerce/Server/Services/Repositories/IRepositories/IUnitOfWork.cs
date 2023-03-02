namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IUnitOfWork
{
	IProductRepository Product { get; }
	ICategoryRepository Category { get; }
	Task Save();
}
