namespace api.Models.DTOs;

public class TokenDTO
{
  public string AccessToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;
  public DateTime ExpiryTime { get; set; }
}
