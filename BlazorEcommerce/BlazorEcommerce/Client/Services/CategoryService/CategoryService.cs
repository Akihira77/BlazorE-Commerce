using BlazorEcommerce.Shared.Models;
using static System.Net.WebRequestMethods;

namespace BlazorEcommerce.Client.Services.CategoryService;

public class CategoryService : ICategoryService
{
	private readonly HttpClient _http;
	public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    public CategoryService(HttpClient http)
    {
		_http = http;
	}
    public async Task GetCategories()
	{
		var result = await _http
		.GetFromJsonAsync<ServiceResponse<IEnumerable<Category>>>("api/v1/Category/Get-all-categories");
		if(result != null && result.Data != null)
		{
			Categories = result.Data;
		}
	}

	public async Task<ServiceResponse<Category>> GetCategory(int id)
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<Category>>($"api/v1/Category/Get-category/{id}");
		return result;
	}
}
