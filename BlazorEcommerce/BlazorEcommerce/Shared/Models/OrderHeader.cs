using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared.Models;
public class OrderHeader
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public User User { get; set; }
	public int? OrderId { get; set; }
	public Order Order { get; set; }

	// false - unpaid
	// true - paid
	public bool IsPaid { get; set; }
	public string SessionId { get; set; } = string.Empty;
}
