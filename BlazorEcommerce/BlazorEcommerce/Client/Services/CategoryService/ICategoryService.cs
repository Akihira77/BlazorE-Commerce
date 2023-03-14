using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Client.Services.CategoryService;

public interface ICategoryService
{
	event Action OnChange;
	IEnumerable<Category> Categories { get; set; }
	List<Category> AdminCategories { get; set; }
	Task<ServiceResponse<Category>> GetCategory(int id);
	Task GetCategories();
	Task GetAdminCategories();
	Task AddCategory(Category category);
	Task UpdateCategory(Category category);
	Task DeleteCategory(int categoryId);
	Category CreateNewCategory();
}
