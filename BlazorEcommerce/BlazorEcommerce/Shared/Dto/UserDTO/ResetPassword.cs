using BlazorEcommerce.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared.Dto.UserDTO;
public class ResetPassword
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string? Token { get; set; } = string.Empty;

    public UserChangePassword UserChange { get; set; }
}

