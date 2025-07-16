using api.Models;
using System.Security.Claims;

namespace api.Services;

public interface ITokenServices
{
  string GenerateAccessToken(User user);
  string GenerateRefreshToken();
  ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
