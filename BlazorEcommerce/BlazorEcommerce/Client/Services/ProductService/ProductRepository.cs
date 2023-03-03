using BlazorEcommerce.Shared;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlazorEcommerce.Client.Services.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly HttpClient _http;
    public event Action ProductsChanged;
    public string Message { get; set; } = "Loading products...";
    public IEnumerable<Product> Products { get; set; } = new List<Product>();

    public ProductRepository(HttpClient http)
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
            "api/v1/Product/Get-all-products" 
            : $"api/v1/Product/Get-products-from/{url}");

        var result = await _http
                    .GetFromJsonAsync
                    <ServiceResponse<IEnumerable<Product>>>(to);
        Products = result.Data;

        ProductsChanged.Invoke();
    }

	public async Task SearchProducts(string searchText)
	{
        var result = await _http
            .GetFromJsonAsync
            <ServiceResponse<IEnumerable<Product>>>
                ($"api/v1/Product/search-products/{searchText}");
        if (result != null && result.Data != null)
        {
            Products = result.Data;
        } 
        if (!Products.Any())
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
