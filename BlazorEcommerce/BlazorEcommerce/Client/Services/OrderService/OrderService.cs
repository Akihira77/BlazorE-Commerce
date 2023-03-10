using Microsoft.AspNetCore.Components;

namespace BlazorEcommerce.Client.Services.OrderService;

public class OrderService : IOrderService
{
	private readonly HttpClient _http;

	public OrderService(HttpClient http)
    {
		_http = http;
	}

	public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsDto>>($"api/v1/order/{orderId}");

		return result.Data;
	}

	public async Task<List<OrderOverviewDto>> GetOrders()
	{
		var result = await _http.GetFromJsonAsync<ServiceResponse<IEnumerable<OrderOverviewDto>>>("api/v1/order");

		return result.Data.ToList();
	}

	public async Task PlaceOrder()
	{
		await _http.PostAsync("api/v1/order", null);
	}
}
