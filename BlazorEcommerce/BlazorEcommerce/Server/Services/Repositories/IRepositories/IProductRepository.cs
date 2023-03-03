namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IProductRepository : IRepository<Product>
{
	void Update(Product obj);
    Task<IEnumerable<Product>> GetProductsByCategory(string? categoryUrl = null);
    Task<Product> GetProduct(int id);
    Task<IEnumerable<Product>> SearchProducts(string searchText);
    List<string> GetProductSearchSuggestion(string searchText);
}
