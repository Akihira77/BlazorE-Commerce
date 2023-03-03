namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductRepository
{
    event Action ProductsChanged;
    IEnumerable<Product> Products { get; set; }
    string Message { get; set; }
    Task GetProducts(string? url = null);
    Task <ServiceResponse<Product>> GetProduct (int id);
    Task SearchProducts(string searchText);
    Task<IEnumerable<string>> SearchProductSuggestion(string searchText);
}
