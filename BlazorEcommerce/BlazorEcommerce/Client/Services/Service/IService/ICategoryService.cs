namespace BlazorEcommerce.Client.Services.Repository.IRepository;

public interface ICategoryService
{
	IEnumerable<Category> Categories { get; set; }
	Task GetCategories();
	Task<ServiceResponse<Category>> GetCategory(int id);
}
