namespace BlazorEcommerce.Shared.Models;
public class CartItem
{
    public int UserId { get; set; }
	public User User { get; set; }
    public int ProductId { get; set; }
	public Product Product { get; set; }
	public int Quantity { get; set; } = 1;
	public int ProductTypeId { get; set; }
    public ProductType ProductType { get; set; }
}
