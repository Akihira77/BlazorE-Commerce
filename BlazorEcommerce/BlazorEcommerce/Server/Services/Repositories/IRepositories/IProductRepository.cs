namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IProductRepository : IRepository<Product>
{
	void Update(Product obj);

	Task<IEnumerable<Product>> GetProductsByCategory(string? categoryUrl = null);

	Task<IEnumerable<Product>> GetProducts();

	Task<Product> GetProduct(int id);

	Task<ProductSearchDto> SearchProducts(string searchText, int page);

	List<string> GetProductSearchSuggestion(string searchText);

	Task<IEnumerable<Product>> GetFeaturedProducts();

	Task<IEnumerable<Product>> GetAdminProducts();

	Task<IEnumerable<ProductRatings>> GetProductRatings(int id);
}