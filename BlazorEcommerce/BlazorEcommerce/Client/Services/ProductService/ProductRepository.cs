namespace BlazorEcommerce.Client.Services.ProductService;

public class ProductRepository : IProductRepository
{
    private readonly HttpClient _http;

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

	public async Task GetProducts()
    {
        var result = await _http
                .GetFromJsonAsync<ServiceResponse<IEnumerable<Product>>>("api/v1/Product/Get-all-products");
        if (result != null && result.Data != null)
        {
            Products = result.Data;
        }
    }
}
