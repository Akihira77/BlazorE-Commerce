using BlazorEcommerce.Client.Pages;
using BlazorEcommerce.Shared.Dto;
using BlazorEcommerce.Shared.Models;
using Blazored.LocalStorage;
using Blazorise;

namespace BlazorEcommerce.Client.Services.CartService;

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
		var carts = await _localStorage.GetItemAsync<List<CartItem>>("cart");
		if (carts != null)
		{ 
			var sameItem = carts.Find(c => c.ProductId == cartItem.ProductId
									&& c.ProductTypeId == cartItem.ProductTypeId);

			if(sameItem != null)
			{
				sameItem.Quantity++;
			} else
			{
				carts.Add(cartItem);
			}
		} else
		{
			carts = new List<CartItem> { cartItem };
			//carts.Add(cartItem);
		} 

		await _localStorage.SetItemAsync("cart", carts);
		OnChange.Invoke();
	}

	public async Task<IEnumerable<CartItem>> GetCartItems()
	{
		var carts = await _localStorage.GetItemAsync<List<CartItem>>("cart");

		carts ??= new List<CartItem>();

		return carts.AsEnumerable();
	}

	public async Task<IEnumerable<CartProductDto>> GetCartProducts()
	{
		var cartItems = await _localStorage.GetItemAsync<IEnumerable<CartItem>>("cart");
		var response = await _http.PostAsJsonAsync("api/v1/cart/products", cartItems);
		var cartProducts = await response
			.Content
			.ReadFromJsonAsync
			<ServiceResponse<IEnumerable<CartProductDto>>>();

		return cartProducts.Data;
	}

	public async Task RemoveProductFromCart(int productId, int productTypeId)
	{
		var carts = await _localStorage.GetItemAsync<List<CartItem>>("cart");

		if (carts == null)
		{
			return;
		} 

		var cartItem = carts
			.FirstOrDefault(x => x.ProductId == productId 
							&& x.ProductTypeId == productTypeId);
		if(cartItem != null)
		{
			carts.Remove(cartItem);
			await _localStorage.SetItemAsync("cart", carts);
			OnChange.Invoke();
		}
	}

	public async Task UpdateQuantity(CartProductDto product, int quantity)
	{
		var carts = await _localStorage.GetItemAsync<List<CartItem>>("cart");
		if (carts == null)
		{
			return;
		}

		var cartItem = carts.FirstOrDefault(c => c.ProductId == product.ProductId
							&& c.ProductTypeId == product.ProductTypeId);
		if(cartItem != null)
		{
			cartItem.Quantity = quantity;
			await _localStorage.SetItemAsync("cart", carts);
			OnChange.Invoke();
		}
	}
}
