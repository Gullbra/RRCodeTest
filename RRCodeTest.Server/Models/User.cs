﻿using Microsoft.AspNetCore.Identity;

namespace RRCodeTest.Server.Models;

public class User : IdentityUser
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many?source=recommendations
    public ICollection<Book> Books { get; } = new List<Book>();
    public ICollection<Quote> Quotes { get; } = new List<Quote>();
}