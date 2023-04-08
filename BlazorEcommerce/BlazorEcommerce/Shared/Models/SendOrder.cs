namespace BlazorEcommerce.Shared.Models;
public class SendOrder
{
	public int Id { get; set; }
	public int UserId { get; set; }
    public User User { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int? AddressId { get; set; }
    public Address Address { get; set; }
    public string Carrier { get; set; } = string.Empty;
    public string TrackingNumber { get; set; } = string.Empty;
    public DateTime? EstimateDate { get; set; } = DateTime.Now.AddDays(7);
}
