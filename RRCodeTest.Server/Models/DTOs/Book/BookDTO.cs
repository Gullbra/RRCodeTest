namespace RRCodeTest.Server.Models.DTOs.Book;

public class BookDTO
{
  public int Id { get; set; }
  public string Title { get; set; } = string.Empty;
  public string Author { get; set; } = string.Empty;
  public DateTime DateOfPublication { get; set; }
  public string UserId { get; set; } = string.Empty;
}
