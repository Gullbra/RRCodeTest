﻿using System.ComponentModel.DataAnnotations;

namespace RRCodeTest.Server.Models.DTOs.User;

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
