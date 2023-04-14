using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared.Models;
public class ProductType
{
	public int Id { get; set; }
	[MaxLength(25)]
	public string Name { get; set; } = string.Empty;
	[NotMapped]
    public bool Editing { get; set; } = false;
    [NotMapped]
    public bool IsNew { get; set; } = false;
    public int CategoryId { get; set; }
}
