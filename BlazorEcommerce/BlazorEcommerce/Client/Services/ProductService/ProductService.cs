namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
	private readonly HttpClient _http;
	public event Action? ProductsChanged;
	public string Message { get; set; } = "Loading products...";
	public int CurrentPage { get; set; } = 1;
	public int PageCount { get; set; } = 0;
	public string LastSearchText { get; set; } = string.Empty;
	public List<Product> Products { get; set; }
	public List<Product> AdminProducts { get; set; }

	public ProductService(HttpClient http)
	{
		_http = http;
	}

	public async Task<ServiceResponse<Product>> GetProduct(int id)
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/v1/Product/Get-product/{id}");
		return result;
	}

	public async Task<ServiceResponse<IEnumerable<ProductRatingsDto>>> GetProductRatings(int id)
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<IEnumerable<ProductRatingsDto>>>($"api/v1/Product/Get-product-ratings/{id}");
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
		Products = result.Data.ToList();
		CurrentPage = 1;
		PageCount = 0;

		if(!Products.Any())
		{
			Message = "No products found.";
		}
		ProductsChanged?.Invoke();
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
			Products = result.Data.Products.ToList();
			CurrentPage = result.Data.CurrentPage;
			PageCount = result.Data.Pages;
		}
		if(!Products.Any())
		{
			Message = "No products found.";
		}

		ProductsChanged?.Invoke();
	}

	public async Task<IEnumerable<string>> SearchProductSuggestion(string searchText)
	{
		var result = await _http
			.GetFromJsonAsync
			<ServiceResponse<IEnumerable<string>>>
				($"api/v1/Product/search-suggestion/{searchText}");

		return result.Data;
	}

	public async Task GetAdminProducts()
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/v1/product/admin");

		AdminProducts = result.Data;
		CurrentPage = 1;
		PageCount = 0;
		if(AdminProducts.Count == 0)
		{
			Message = "No products found.";
		}
	}

	public async Task<Product> CreateProduct(Product product)
	{
		var result = await _http.PostAsJsonAsync("api/v1/product/add-product", product);
		var newProduct = (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;

		return newProduct;
	}

	public async Task<Product> UpdateProduct(Product product)
	{
		var result = await _http.PutAsJsonAsync("api/v1/product/update-product", product);

		return (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
	}

	public async Task<ServiceResponse<bool>> DeleteProduct(Product product)
	{
		var result = await _http.DeleteAsync($"api/v1/product/delete-product/{product.Id}");

		return (await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>());

	}
}
