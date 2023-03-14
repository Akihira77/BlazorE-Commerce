using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEcommerce.Shared.Models;
public class User
{
	public int Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public byte[] PasswordHash { get; set; }
	public byte[] PasswordSalt { get; set; }
	public DateTime DateCreated { get; set; } = DateTime.Now;
    public Address Address { get; set; }
	public string Role { get; set; } = "Customer";

    [NotMapped]
	public string Message { get; set; } = "User registration failed";
}
