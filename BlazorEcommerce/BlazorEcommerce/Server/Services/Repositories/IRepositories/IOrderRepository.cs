namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IOrderRepository : IRepository<Order>
{
	Order PlaceOrder(IEnumerable<CartProductDto> cartProducts);
	Task<IEnumerable<OrderOverviewDto>> GetOrders();
	Task<OrderDetailsDto> GetOrderDetails(int orderId);
}
