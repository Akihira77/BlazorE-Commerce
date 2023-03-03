using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Server.Services.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
	private readonly AppDbContext _db;

	public CategoryRepository(AppDbContext db) : base(db)
	{
		_db = db;
	}

	public void Update(Category obj)
	{
		_db.Categories.Update(obj);
	}
}
