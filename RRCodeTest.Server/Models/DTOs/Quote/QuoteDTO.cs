namespace RRCodeTest.Server.Models.DTOs.Quote;

public class QuoteDTO
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
}