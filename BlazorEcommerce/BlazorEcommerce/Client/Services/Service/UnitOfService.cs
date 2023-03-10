using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Services.Repository;

public class UnitOfService : IUnitOfService
{
    public IAuthService Auth { get; private set; }
    public IOrderService Order { get; private set; }
    public IProductService Product { get; private set; }
    public ICategoryService Category { get; private set; }
    public ICartService Cart { get; private set; }

    public UnitOfService(HttpClient http, ILocalStorageService localStorageService)
    {
        Auth = new AuthService(http);
        Category = new CategoryService(http);
        Cart = new CartService(localStorageService, http);
        Product = new ProductService(http);
        Order = new OrderService(http);
    }
}
