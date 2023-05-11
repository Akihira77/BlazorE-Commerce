using BlazorEcommerce.Server.Features.Base;
using BlazorEcommerce.Shared.Dto.CartDTO;

namespace BlazorEcommerce.Server.Features.Carts;

public interface ICartRepository : IRepository<CartItem>
{
    Task<IEnumerable<CartProductDto>> GetCartProducts(IEnumerable<CartItem> cartItems);

    IEnumerable<CartItem> StoreCartItems(IEnumerable<CartItem> cartItems);

    Task<int> GetCartItemsCount();
}