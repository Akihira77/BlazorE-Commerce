using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorEcommerce.Shared.Models;
public class ProductRatings
{
    public User User { get; set; }
	[Required]
    public int UserId { get; set; }
	[JsonIgnore]
	public Product Product { get; set; }
	[Required]
    public int ProductId { get; set; }
    [Range(0, 5)]
    public int Rate { get; set; } = 0;
    public string? Reviews { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
}
