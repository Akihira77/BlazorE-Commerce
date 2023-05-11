using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared.Dto.UserDTO;
public class UserLogin
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}
