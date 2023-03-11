namespace BlazorEcommerce.Client.Services.CategoryService;

public interface ICategoryService
{
	IEnumerable<Category> Categories { get; set; }
	Task GetCategories();
	Task<ServiceResponse<Category>> GetCategory(int id);
}
