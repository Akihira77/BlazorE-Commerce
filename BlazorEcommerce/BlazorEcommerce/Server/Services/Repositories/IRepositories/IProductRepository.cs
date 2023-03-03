namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IProductRepository : IRepository<Product>
{
	void Update(Product obj);
    public Task<IEnumerable<Product>> GetProductsByCategory(string? categoryUrl = null);
    public Task<Product> GetProduct(int id);
}
