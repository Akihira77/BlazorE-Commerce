namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface ICategoryRepository : IRepository<Category>
{
	void Update(Category obj);
}
