﻿using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs;

public class RegisterDTO
{
  [Required]
  [EmailAddress]
  public string Email { get; set; } = string.Empty;

  [Required]
  [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
  public string Password { get; set; } = string.Empty;

  [Required]
  [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
  public string ConfirmPassword { get; set; } = string.Empty;
}
