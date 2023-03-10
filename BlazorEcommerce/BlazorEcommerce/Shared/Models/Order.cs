using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared.Models;
public class Order
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public DateTime OrderDate { get; set; } = DateTime.Now;
	[Column(TypeName = "decimal(18,2)")]
	public decimal TotalPrice { get; set; }
	public IEnumerable<OrderItem> OrderItems { get; set; }
}
