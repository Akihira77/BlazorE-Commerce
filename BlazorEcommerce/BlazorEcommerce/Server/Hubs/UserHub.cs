using Microsoft.AspNetCore.SignalR;

namespace BlazorEcommerce.Server.Hubs;

public class UserHub : Hub
{
	public static int TotalViews { get; set; } = 0;

	public override async Task OnDisconnectedAsync(Exception? exception)
	{
		await base.OnDisconnectedAsync(exception);
	}

	public async Task CalculateUserView()
	{
		TotalViews++;
		await Clients.All.SendAsync("updateUserViewCount", TotalViews);
	}

	public async Task AdminNotificationAfterUserCheckout(string message)
	{
		await Clients.All.SendAsync("userCheckout", message);
	}

	public async Task SendMessageToAllClients(string message)
	{
		await Clients.All.SendAsync("ReceiveMessages", message);
	}
}