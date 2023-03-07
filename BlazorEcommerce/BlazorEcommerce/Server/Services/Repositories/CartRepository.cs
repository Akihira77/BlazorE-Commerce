using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.Repositories.IRepositories;

namespace BlazorEcommerce.Server.Services.Repositories;

public class CartRepository : Repository<CartItem>, ICartRepository
{
	private readonly AppDbContext _db;

	public CartRepository(AppDbContext db) : base(db)
	{
		_db = db;
	}

	public async Task<IEnumerable<CartProductDto>> GetCartProducts(IEnumerable<CartItem> cartItems)
	{
		var result = new List<CartProductDto>();

		foreach(var cartItem in cartItems)
		{
			var product = await _db.Products
					.Where(p => p.Id == cartItem.ProductId)
					.FirstOrDefaultAsync();

			if(product == null)
			{
				continue;
			}

			var productVariant = await _db.ProductVariants
					.Where(v => v.ProductId == cartItem.ProductId
					&& v.ProductTypeId == cartItem.ProductTypeId)
					.Include(v => v.ProductType)
					.FirstOrDefaultAsync();

			if(productVariant == null)
			{
				continue;
			}

			var carProduct = new CartProductDto
			{
				ProductId = product.Id,
				Title = product.Title,
				ImageUrl = product.ImageUrl,
				Price = productVariant.Price,
				ProductType = productVariant.ProductType.Name,
				ProductTypeId = productVariant.ProductTypeId,
				Quantity = cartItem.Quantity,
			};

			result.Add(carProduct);
		}
		return result.AsEnumerable();
	}
}
