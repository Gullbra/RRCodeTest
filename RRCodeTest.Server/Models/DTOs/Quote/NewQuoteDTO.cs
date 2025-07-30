using System.ComponentModel.DataAnnotations;

namespace RRCodeTest.Server.Models.DTOs.Quote;

public class NewQuoteDTO
{
    [Required]
    public string Text { get; set; } = string.Empty;
    [Required]
    public string Author { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
}
