using BlazorEcommerce.Shared;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BlazorEcommerce.Client.Services.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly HttpClient _http;
    public event Action ProductsChanged;

    public ProductRepository(HttpClient http)
    {
        _http = http;
    }
    public IEnumerable<Product> Products { get; set; } = new List<Product>();


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
                    .GetFromJsonAsync<ServiceResponse<IEnumerable<Product>>>(to);
        Products = result.Data;

        ProductsChanged.Invoke();
    }
}
