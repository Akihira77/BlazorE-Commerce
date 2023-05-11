using BlazorEcommerce.Shared.Dto.OrderDTO;

namespace BlazorEcommerce.Client.Services.OrderService;

public class OrderService : IOrderService
{
	private readonly HttpClient _http;

    public event Action? OnChange;

    public List<OrderDto> AdminOrders { get; set; } = new List<OrderDto>();

    public OrderService(HttpClient http)
    {
		_http = http;
	}
    public async Task GetAdminOrders()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<IEnumerable<OrderDto>>>("api/v1/order/admin-orders");
        AdminOrders = result.Data.ToList();
    }
    public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsDto>>($"api/v1/order/{orderId}");

		return result.Data;
	}

	public async Task<OrderDetailsDto> AdminGetOrderDetails(int orderId)
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsDto>>($"api/v1/order/admin/{orderId}");

		return result.Data;
	}

	public async Task<List<OrderOverviewDto>> GetOrders()
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<IEnumerable<OrderOverviewDto>>>("api/v1/order");

		return result.Data.ToList();
	}

	public async Task<string> PlaceOrder()
	{
		var result = await _http.PostAsync("api/v1/payment/checkout", null);
		var url = await result.Content.ReadAsStringAsync();

		return url;
	}

	public async Task<bool> Invoice(int orderId)
	{
		var result = await _http.GetFromJsonAsync<bool>($"api/v1/order/invoice/{orderId}");

		return result;
	}

	public async Task<OrderOverviewDto> GetOrder(int orderId)
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<OrderOverviewDto>>($"api/v1/order/admin/get-order/{orderId}");

		return result.Data;
	}

	public async Task UpdateOrderStatus(int orderId, int orderStatus)
	{
		var result = await _http.PutAsJsonAsync($"api/v1/order/admin/update-order-status/{orderId}", orderStatus);

		//AdminOrders = (await result.Content
		//			.ReadFromJsonAsync<ServiceResponse<IEnumerable<OrderDto>>>())
		//			.Data.ToList();
		OnChange?.Invoke();
	}

    public async Task SendOrder(int orderId)
    {
		await _http.PostAsJsonAsync("api/v1/order/admin/send-order", orderId);
    }

	public async Task CancelOrder(int orderId)
	{
		var result = await _http.DeleteAsync($"api/v1/order/cancel-order/{orderId}");

		AdminOrders = (await result.Content
					.ReadFromJsonAsync<ServiceResponse<IEnumerable<OrderDto>>>())
					.Data.ToList();
		OnChange?.Invoke();
	}

    public async Task<Order> GetOrderModel(int orderId)
    {
		var result = await _http.GetFromJsonAsync<ServiceResponse<Order>>($"api/v1/order/admin/get-order-model/{orderId}");

		return result.Data;
    }
}
