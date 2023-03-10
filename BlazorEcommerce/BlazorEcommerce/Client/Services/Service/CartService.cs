using Blazored.LocalStorage;
namespace BlazorEcommerce.Client.Services.Repository;

public class CartService : ICartService
{
	private readonly ILocalStorageService _localStorage;
	private readonly HttpClient _http;

	public event Action OnChange;

	public CartService(ILocalStorageService localStorage, HttpClient http)
	{
		_localStorage = localStorage;
		_http = http;
	}

	public async Task AddToCart(CartItem cartItem)
	{

		await _http.PostAsJsonAsync("api/v1/cart/add", cartItem);
		await GetCartItemsCount();
	}

	public async Task<IEnumerable<CartProductDto>> GetCartProducts()
	{
		await GetCartItemsCount();
		var response = await _http.GetFromJsonAsync<ServiceResponse<IEnumerable<CartProductDto>>>("api/v1/cart");
		return response.Data;
	}

	public async Task RemoveItemFromCart(int productId, int productTypeId)
	{
		await _http.DeleteAsync($"api/v1/cart/remove-cart-item/{productId}&&{productTypeId}");
	}

	public async Task UpdateQuantity(CartProductDto product, int quantity)
	{
		await _http.PostAsJsonAsync("api/v1/cart/add",
			new CartItem
			{
				ProductId = product.ProductId,
				ProductTypeId = product.ProductTypeId,
				Quantity = quantity
			});
	}

	public async Task StoreCartItems(bool emptyLocalCart = false)
	{
		var localCart = await _localStorage.GetItemAsync<IEnumerable<CartItem>>("cart");
		if(localCart == null)
		{
			return;
		}

		await _http.PostAsJsonAsync("api/v1/cart", localCart);

		if(emptyLocalCart)
		{
			await _localStorage.RemoveItemAsync("cart");
		}
	}

	public async Task GetCartItemsCount()
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/v1/cart/count");
		var count = result.Data;

		await _localStorage.SetItemAsync("cartItemsCount", count);

		OnChange.Invoke();
	}
}
