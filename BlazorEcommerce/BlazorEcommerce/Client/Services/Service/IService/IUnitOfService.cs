namespace BlazorEcommerce.Client.Services.Repository.IRepository;

public interface IUnitOfService
{
    IAuthService Auth { get; }
    IOrderService Order { get; }
    IProductService Product { get; }
    ICategoryService Category { get; }
    ICartService Cart { get; }
}
