namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface ICartRepository : IRepository<CartItem>
{
	Task<IEnumerable<CartProductDto>> GetCartProducts(IEnumerable<CartItem> cartItems);

	IEnumerable<CartItem> StoreCartItems(IEnumerable<CartItem> cartItems);

	Task<int> GetCartItemsCount();
}