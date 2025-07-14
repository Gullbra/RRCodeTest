using System.ComponentModel.DataAnnotations;

namespace api.Models.DTOs;

public class RefreshTokenDTO
{
  // Does this make sense?
  [Required]
  public string AccessToken { get; set; } = string.Empty;

  [Required]
  public string RefreshToken { get; set; } = string.Empty;
}
