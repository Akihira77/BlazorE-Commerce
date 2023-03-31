namespace BlazorEcommerce.Shared.Models;
public class OrderHeader
{
	public int Id { get; set; }
    public int UserId { get; set; }
    public string SessionId { get; set; } = string.Empty;
}
