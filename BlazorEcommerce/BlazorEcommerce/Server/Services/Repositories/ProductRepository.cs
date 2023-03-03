using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore.Query;

namespace BlazorEcommerce.Server.Services.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
	private readonly AppDbContext _db;

	public ProductRepository(AppDbContext db) : base(db)
	{
		_db = db;
	}

    public async Task<Product> GetProduct(int id)
    {
        return await _db.Products
                .Include(p => p.Variants)
                .ThenInclude(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetProductsByCategory(string? categoryUrl = null)
    {
		return await _db.Products
				.Where(p => p.Category.Url == categoryUrl)
				.Include(p => p.Variants)
				.ThenInclude(p => p.ProductType)
				.ToListAsync();
    }

	public List<string> GetProductSearchSuggestion(string searchText)
	{
		var obj = FindProductsBySearch(searchText);

		List<string> result = new();

		foreach (var item in obj)
		{
			if (item.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
			{
				result.Add(item.Title);
			}

			if (item.Description != null)
			{
				var punctuation = item.Description
					.Where(char.IsPunctuation)
					.Distinct()
					.ToArray();
				var words = item.Description
					.Split()
					.Select(s => s.Trim(punctuation));

				foreach(var word in words)
				{
					if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
						&& !result.Contains(word))
					{
						result.Add(word);
					} 
				}
			} 
		}

		return result;
	}

	public async Task<IEnumerable<Product>> SearchProducts(string searchText)
	{
		var obj = FindProductsBySearch(searchText);

		return await obj.ToListAsync();
	}

	private IIncludableQueryable<Product, IEnumerable<ProductVariant>> FindProductsBySearch(string searchText)
	{
		return _db.Products
			.Where(p => p.Title.ToLower().Contains(searchText.ToLower())
			|| p.Description.ToLower().Contains(searchText.ToLower()))
			.Include(p => p.Variants);
	}

	public void Update(Product obj)
	{
		_db.Products.Update(obj);
	}
}
