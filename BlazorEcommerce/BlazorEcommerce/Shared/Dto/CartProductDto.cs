using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Shared.Dto;
public class CartProductDto
{
	public int ProductId { get; set; }
	public string Title { get; set; } = string.Empty;
	public int ProductTypeId { get; set; }
	public string ProductType { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }

	public int Quantity { get; set; }
}
