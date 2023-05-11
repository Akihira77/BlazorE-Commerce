﻿using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared.Dto.UserDTO;
public class UserRegister
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
    [Required, Compare("Password", ErrorMessage = "The passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
