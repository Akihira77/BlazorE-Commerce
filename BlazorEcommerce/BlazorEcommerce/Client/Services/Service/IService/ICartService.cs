﻿namespace BlazorEcommerce.Client.Services.Repository.IRepository;

public interface ICartService
{
    event Action OnChange;
    Task AddToCart(CartItem cartItem);
    Task<IEnumerable<CartProductDto>> GetCartProducts();
    Task RemoveItemFromCart(int productId, int productTypeId);
    Task UpdateQuantity(CartProductDto product, int quantity);
    Task StoreCartItems(bool emptyLocalCart = false);
    Task GetCartItemsCount();
}
