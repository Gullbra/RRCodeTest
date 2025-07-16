using System.ComponentModel.DataAnnotations;

namespace RRCodeTest.Server.Models.DTOs.Token;

public class RefreshTokenDTO
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;

    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
