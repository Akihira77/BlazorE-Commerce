namespace BlazorEcommerce.Client.Services.ProductTypeService;

public interface IProductTypeService
{
    event Action? OnChange;
    public List<ProductType> ProductTypes { get; set; }
    Task GetProductTypes();
    Task GetProductTypesByCategory(int categoryId);
    Task UpdateProductTypes(ProductType productType);
    Task AddProductTypes(ProductType productType);
    Task DeleteProductTypes(int productTypeId);
    ProductType CreateNewProductType();
}
