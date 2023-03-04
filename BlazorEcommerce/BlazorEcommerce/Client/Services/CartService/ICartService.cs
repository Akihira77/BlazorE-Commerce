namespace BlazorEcommerce.Client.Services.CartService;

public interface ICartService
{
	event Action OnChange;
	Task AddToCart(CartItem cartItem);
	Task<IEnumerable<CartItem>> GetCartItems();
	Task<IEnumerable<CartProductDto>> GetCartProducts();
	Task RemoveProductFromCart(int productId, int productTypeId);
	Task UpdateQuantity(CartProductDto product, int quantity, char? opt = null);
}
