namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IProductRepository : IRepository<Product>
{
	void Update(Product obj);
}
