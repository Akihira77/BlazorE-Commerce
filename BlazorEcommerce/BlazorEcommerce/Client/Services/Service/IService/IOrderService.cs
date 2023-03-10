namespace BlazorEcommerce.Client.Services.Repository.IRepository;

public interface IOrderService
{
	Task<string> PlaceOrder();
	Task<List<OrderOverviewDto>> GetOrders();
	Task<OrderDetailsDto> GetOrderDetails(int orderId);
}
