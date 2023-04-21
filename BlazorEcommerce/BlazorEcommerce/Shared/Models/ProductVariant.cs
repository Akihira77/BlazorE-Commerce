using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorEcommerce.Shared.Models;
public class ProductVariant
{
	[JsonIgnore]
	public Product? Product { get; set; }
	[Required]
	public int ProductId { get; set; }

	public ProductType? ProductType { get; set; }
	[Required]
	public int ProductTypeId { get; set; }

	[Column(TypeName = "decimal(18,2)")]
	[Required]
	public decimal Price { get; set; }
	[Column(TypeName = "decimal(18,2)")]
	public decimal OriginalPrice { get; set; }

	public bool Visible { get; set; } = true;
	public bool Deleted { get; set; } = false;

	[NotMapped]
	public bool Editing { get; set; } = false;
	[NotMapped]
	public bool IsNew { get; set; } = false;
}
