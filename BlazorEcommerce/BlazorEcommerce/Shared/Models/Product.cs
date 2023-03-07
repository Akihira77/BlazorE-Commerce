using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared.Models;
public class Product
{
	[Key]
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
	[ForeignKey("CategoryId")]
	public Category Category { get; set; }
	public int CategoryId { get; set; }

	public bool Featured { get; set; } = false;

	public IEnumerable<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
}
