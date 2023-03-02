namespace BlazorEcommerce.Client.Services.ProductService;

public interface IProductRepository
{
    IEnumerable<Product> Products { get; set; }
    Task GetProducts();
    Task <ServiceResponse<Product>> GetProduct (int id);
}
