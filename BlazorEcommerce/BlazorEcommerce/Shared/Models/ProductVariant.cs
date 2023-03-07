using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorEcommerce.Shared.Models;
public class ProductVariant
{
	[ForeignKey("ProductId")]
	[JsonIgnore]
	public Product Product { get; set; }
	public int ProductId { get; set; }

	[ForeignKey("ProductTypeId")]
	public ProductType ProductType { get; set; }
	public int ProductTypeId { get; set; }

	[Column(TypeName = "decimal(18,2)")]
	public decimal Price { get; set; }
	[Column(TypeName = "decimal(18,2)")]
	public decimal OriginalPrice { get; set; }
}
