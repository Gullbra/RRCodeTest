using System.ComponentModel.DataAnnotations;

namespace RRCodeTest.Server.Models.DTOs.Book;

public class NewBookDTO
{
  [Required]
  public string Title { get; set; } = string.Empty;
  [Required]
  public string Author { get; set; } = string.Empty;
  [Required]
  public DateTime DateOfPublication { get; set; }
}
