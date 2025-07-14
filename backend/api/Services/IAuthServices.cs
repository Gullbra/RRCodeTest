using api.Models;
using api.Models.DTOs;

namespace api.Services;

public interface IAuthServices
{
  Task<ApiResponse<TokenDTO>> Login(LoginDTO loginInfo);
  Task<ApiResponse<UserDTO>> Register(RegisterDTO registerInfo);
  Task<ApiResponse<TokenDTO>> RefreshToken(RefreshTokenDTO refreshInfo);
  Task<ApiResponse<bool>> Logout(string userId);
}