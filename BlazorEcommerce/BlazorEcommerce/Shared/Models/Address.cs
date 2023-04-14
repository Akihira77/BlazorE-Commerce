using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared.Models;
public class Address
{
	public int Id { get; set; }
	public int UserId { get; set; }
	[MaxLength(10)]
	public string FirstName { get; set; } = string.Empty;
	[MaxLength(10)]
	public string LastName { get; set; } = string.Empty;
	[MaxLength(50)]
	public string Street { get; set; } = string.Empty;
	[MaxLength(25)]
	public string City { get; set; } = string.Empty;
	[MaxLength(25)]
	public string State { get; set; } = string.Empty;
	[MaxLength(15)]
	public string Zip { get; set; } = string.Empty;
	[MaxLength(25)]
	public string Country { get; set; } = string.Empty;
}
