using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared.Models;
public class User
{
	public int Id { get; set; }
	[Required, EmailAddress]
	public string Email { get; set; } = string.Empty;
	public byte[] PasswordHash { get; set; }
	public byte[] PasswordSalt { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.Now;
    public Address Address { get; set; }
	[MaxLength(10)]
	public string Role { get; set; } = "Customer";

    [NotMapped]
	public string Message { get; set; } = "User registration failed";
}
