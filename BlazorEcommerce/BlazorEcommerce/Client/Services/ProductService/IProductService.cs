namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductService
{
	event Action ProductsChanged;
	IEnumerable<Product> Products { get; set; }
	string Message { get; set; }
	int CurrentPage { get; set; }
	int PageCount { get; set; }
	string LastSearchText { get; set; }
	Task GetProducts(string? url = null);
	Task<ServiceResponse<Product>> GetProduct(int id);
	Task SearchProducts(string searchText, int page);
	Task<IEnumerable<string>> SearchProductSuggestion(string searchText);
}
