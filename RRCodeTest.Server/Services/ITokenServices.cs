using RRCodeTest.Server.Models;
using System.Security.Claims;

namespace RRCodeTest.Server.Services;

public interface ITokenServices
{
  string GenerateAccessToken(User user);
  string GenerateRefreshToken();
  ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
