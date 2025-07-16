namespace api.Models.DTOs.Book;

    //public string Id { get; set; } = string.Empty;
    //public string Title { get; set; } = string.Empty;
    //public string Author { get; set; } = string.Empty;
    //public string Description { get; set; } = string.Empty;
    //public string User { get; set; } = string.Empty;
    //public DateTime DateOfPublication { get; set; }
public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime DateOfPublication { get; set; }
    public string UserId { get; set; } = string.Empty;
}
