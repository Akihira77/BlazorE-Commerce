namespace BlazorEcommerce.Client.Services.OrderService;

public interface IOrderService
{
	event Action? OnChange;
	List<OrderDto> AdminOrders { get; set; }
	Task<string> PlaceOrder();
	Task<List<OrderOverviewDto>> GetOrders();
	Task<OrderOverviewDto> GetOrder(Guid orderId);
	Task<OrderDetailsDto> GetOrderDetails(Guid orderId);
	Task<OrderDetailsDto> AdminGetOrderDetails(Guid orderId);
	Task<Order> GetOrderModel(Guid orderId);
	Task<bool> Invoice(Guid orderId);
	Task GetAdminOrders();
	Task UpdateOrderStatus(Guid orderId, int orderStatus);
	Task SendOrder(Guid orderId);
	Task CancelOrder(Guid orderId);
}
