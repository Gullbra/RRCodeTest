//using Microsoft.AspNetCore.Identity;

namespace api.Models;

//public class Book : IdentityUser
//{
//  public string Title { get; set; } = string.Empty;
//  public string Author { get; set; } = string.Empty;
//  public string Description { get; set; } = string.Empty;
//  public DateTime DateOfPublication { get; set; }
//  public string User { get; set; } = string.Empty;
//}


//using System.ComponentModel.DataAnnotations;

//public class Book
//{
//  public int Id { get; set; }

//  [Required]
//  [StringLength(200)]
//  public string Title { get; set; }

//  [Required]
//  [StringLength(100)]
//  public string Author { get; set; }

//  [StringLength(13)]
//  public string ISBN { get; set; }

//  public DateTime PublishedDate { get; set; }

//  public DateTime CreatedAt { get; set; } = DateTime.Now;

//  // Foreign key for ApplicationUser
//  public string UserId { get; set; }
//  public virtual User User { get; set; }
//}

public class Book
{
  public int Id { get; set; }
  public string Title { get; set; } 
  public string Author { get; set; }
  public DateTime DateOfPublication { get; set; }

  public string UserId { get; set; }
  public virtual User User { get; set; } = null!;
}

// https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many?source=recommendations