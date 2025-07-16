using RRCodeTest.Server.Models;
using RRCodeTest.Server.Models.DTOs.Token;
using RRCodeTest.Server.Models.DTOs.User;

namespace RRCodeTest.Server.Services;

public interface IAuthServices
{
  Task<ApiResponse<TokenDTO>> Login(LoginDTO loginInfo);
  Task<ApiResponse<UserDTO>> Register(RegisterDTO registerInfo);
  Task<ApiResponse<TokenDTO>> RefreshToken(RefreshTokenDTO refreshInfo);
  Task<ApiResponse<bool>> Logout(string userId);
}