using BlazorEcommerce.Shared.Dto.OrderDTO;

namespace BlazorEcommerce.Client.Services.OrderService;

public interface IOrderService
{
	event Action? OnChange;
	List<OrderDto> AdminOrders { get; set; }
	Task<string> PlaceOrder();
	Task<List<OrderOverviewDto>> GetOrders();
	Task<OrderOverviewDto> GetOrder(int orderId);
	Task<OrderDetailsDto> GetOrderDetails(int orderId);
	Task<OrderDetailsDto> AdminGetOrderDetails(int orderId);
	Task<Order> GetOrderModel(int orderId);
	Task<bool> Invoice(int orderId);
	Task GetAdminOrders();
	Task UpdateOrderStatus(int orderId, int orderStatus);
	Task SendOrder(int orderId);
	Task CancelOrder(int orderId);
}
