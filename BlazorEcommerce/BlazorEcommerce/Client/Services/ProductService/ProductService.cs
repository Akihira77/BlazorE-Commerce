namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
	private readonly HttpClient _http;
	public event Action ProductsChanged;
	public string Message { get; set; } = "Loading products...";
	public int CurrentPage { get; set; } = 1;
	public int PageCount { get; set; } = 0;
	public string LastSearchText { get; set; } = string.Empty;
	public IEnumerable<Product> Products { get; set; } = new List<Product>();

	public ProductService(HttpClient http)
	{
		_http = http;
	}

	public async Task<ServiceResponse<Product>> GetProduct(int id)
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/v1/Product/Get-product/{id}");
		return result;
	}

	public async Task GetProducts(string? url = null)
	{
		string to = (url == null ?
			"api/v1/product/featured"
			: $"api/v1/product/get-products-from/{url}");

		var result = await _http
					.GetFromJsonAsync
					<ServiceResponse<IEnumerable<Product>>>(to);
		Products = result.Data;
		CurrentPage = 1;
		PageCount = 0;

		if(!Products.Any())
		{
			Message = "No products found.";
		}
		ProductsChanged.Invoke();
	}

	public async Task SearchProducts(string searchText, int page)
	{
		LastSearchText = searchText;
		var result = await _http
			.GetFromJsonAsync
			<ServiceResponse<ProductSearchDto>>
				($"api/v1/product/search-products/{searchText}/{page}");
		if(result != null && result.Data != null)
		{
			Products = result.Data.Products;
			CurrentPage = result.Data.CurrentPage;
			PageCount = result.Data.Pages;
		}
		if(!Products.Any())
		{
			Message = "No products found.";
		}

		ProductsChanged.Invoke();
	}

	public async Task<IEnumerable<string>> SearchProductSuggestion(string searchText)
	{
		var result = await _http
			.GetFromJsonAsync
			<ServiceResponse<IEnumerable<string>>>
				($"api/v1/Product/search-suggestion/{searchText}");

		return result.Data;
	}
}
