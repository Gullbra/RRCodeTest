namespace RRCodeTest.Server.Models;


public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime DateOfPublication { get; set; }

    // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many?source=recommendations
    public string UserId { get; set; } = string.Empty;
    public virtual User User { get; set; } = null!;
}
