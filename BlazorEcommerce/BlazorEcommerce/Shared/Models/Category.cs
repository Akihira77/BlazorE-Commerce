using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared.Models;
public class Category
{
	[Key]
	public int Id { get; set; }
	[MaxLength(20)]
	public string Name { get; set; } = string.Empty;
	[MaxLength(20)]
	public string Url { get; set; } = string.Empty;
	public bool Visible { get; set; } = true;
	public bool Deleted { get; set; } = false;

	[NotMapped]
	public bool Editing { get; set; } = false;
	[NotMapped]
	public bool IsNew { get; set; } = false;
}
