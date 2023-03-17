using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Client.Services.ProductTypeService;

public class ProductTypeService : IProductTypeService
{
    private readonly HttpClient _http;

    public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();

    public event Action OnChange;
    public ProductTypeService(HttpClient http)
    {
        _http = http;
    }
    public async Task GetProductTypes()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/v1/ProductType/get-all-producttype");
        ProductTypes = result.Data;
    }

	public async Task UpdateProductTypes(ProductType productType)
	{
		var response = await _http.PutAsJsonAsync("api/v1/producttype/update-producttype", productType);
		ProductTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
		OnChange.Invoke();
	}

	public async Task AddProductTypes(ProductType productType)
	{
		var response = await _http.PostAsJsonAsync("api/v1/producttype/add-producttype", productType);
		ProductTypes = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
		OnChange.Invoke();
	}

	public ProductType CreateNewProductType()
	{
		var newProductType = new ProductType() { IsNew = true, Editing = true };

		ProductTypes.Add(newProductType);
		OnChange.Invoke();
		return newProductType;
	}

	public async Task DeleteProductTypes(int productTypeId)
	{
		var response = await _http.DeleteAsync($"api/v1/producttype/delete-producttype/{productTypeId}");
		ProductTypes = (await response.Content
							.ReadFromJsonAsync<ServiceResponse<IEnumerable<ProductType>>>()).Data.ToList();

		await GetProductTypes();
		OnChange.Invoke();
	}
}
