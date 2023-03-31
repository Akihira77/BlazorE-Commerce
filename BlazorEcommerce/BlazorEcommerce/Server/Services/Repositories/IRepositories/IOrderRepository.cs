namespace BlazorEcommerce.Server.Services.Repositories.IRepositories;

public interface IOrderRepository : IRepository<Order>
{
	Order PlaceOrder(IEnumerable<CartProductDto> cartProducts, int? userId = null);
	Task<IEnumerable<OrderOverviewDto>> GetOrders();
	Task<OrderDetailsDto> GetOrderDetails(int orderId);
	Task<OrderDetailsDto> AdminGetOrderDetails(int orderId);
}
