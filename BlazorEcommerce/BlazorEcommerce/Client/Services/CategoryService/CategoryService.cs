namespace BlazorEcommerce.Client.Services.CategoryService;

public class CategoryService : ICategoryService
{
	private readonly HttpClient _http;

	public event Action OnChange;

	public IEnumerable<Category> Categories { get; set; } = new List<Category>();
	public List<Category> AdminCategories { get; set; } = new List<Category>();

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

	public async Task GetAdminCategories()
	{
		var result = await _http
.GetFromJsonAsync<ServiceResponse<IEnumerable<Category>>>("api/v1/Category/admin");
		if(result != null && result.Data != null)
		{
			AdminCategories = result.Data.ToList();
		}
	}

	public async Task AddCategory(Category category)
	{
		var response = await _http.PostAsJsonAsync("api/v1/category/admin", category);
		AdminCategories = (await response.Content
							.ReadFromJsonAsync<ServiceResponse<IEnumerable<Category>>>()).Data.ToList();

		await GetCategories();
		OnChange.Invoke();
	}

	public async Task UpdateCategory(Category category)
	{
		var response = await _http.PutAsJsonAsync("api/v1/category/admin", category);
		AdminCategories = (await response.Content
							.ReadFromJsonAsync<ServiceResponse<IEnumerable<Category>>>()).Data.ToList();

		await GetCategories();
		OnChange.Invoke();
	}

	public async Task DeleteCategory(int categoryId)
	{
		var response = await _http.DeleteAsync($"api/v1/category/admin/{categoryId}");
		AdminCategories = (await response.Content
							.ReadFromJsonAsync<ServiceResponse<IEnumerable<Category>>>()).Data.ToList();

		await GetCategories();
		OnChange.Invoke();
	}

	public Category CreateNewCategory()
	{
		var newCategory = new Category { IsNew = true, Editing = true };
		AdminCategories.Add(newCategory);
		OnChange.Invoke();
		return newCategory;
	}
}
