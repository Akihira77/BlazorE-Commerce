namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductService
{
	event Action? ProductsChanged;
	List<Product> Products { get; set; }
	List<Product> AdminProducts { get; set; }
	string Message { get; set; }
	int CurrentPage { get; set; }
	int PageCount { get; set; }
	string LastSearchText { get; set; }
	Task GetProducts(string? url = null);
	Task<ServiceResponse<Product>> GetProduct(int id);
	Task SearchProducts(string searchText, int page);
	Task<IEnumerable<string>> SearchProductSuggestion(string searchText);
	Task GetAdminProducts();
	Task<Product> CreateProduct(Product product);
	Task<Product> UpdateProduct(Product product);
	Task<ServiceResponse<bool>> DeleteProduct(Product product);
}
