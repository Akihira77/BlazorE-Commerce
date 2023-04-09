namespace BlazorEcommerce.Shared.Models;
public class OrderHeader
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public User User { get; set; }
	public int? OrderId { get; set; }
	public Order Order { get; set; }

	// 0 unpaid
	// 1 paid
	public int PaymentStatus { get; set; } = 1;
	public string SessionId { get; set; } = string.Empty;
}
