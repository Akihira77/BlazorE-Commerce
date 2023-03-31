namespace BlazorEcommerce.Shared.Dto;
public class OrderDetailsDto
{
	public DateTime OrderDate { get; set; }
	public decimal TotalPrice { get; set; }
	public IEnumerable<OrderDetailsProductDto> Products { get; set; }
    public int OrderStatus { get; set; }
}
