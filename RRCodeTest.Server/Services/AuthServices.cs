using RRCodeTest.Server.Models;
using RRCodeTest.Server.Models.DTOs.Token;
using RRCodeTest.Server.Models.DTOs.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace RRCodeTest.Server.Services;

public class AuthServices : IAuthServices
{
  private readonly UserManager<User> _userManager;
  private readonly ITokenServices _tokenServices;
  private readonly IConfiguration _configuration;


  public AuthServices(UserManager<User> userManager, ITokenServices tokenServices, IConfiguration configuration)
  {
    _userManager = userManager;
    _tokenServices = tokenServices;
    _configuration = configuration;
  }


  private async Task<TokenDTO> CreateTokenDTO(User user)
  {
    var accessToken = _tokenServices.GenerateAccessToken(user);
    var refreshToken = _tokenServices.GenerateRefreshToken();

    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtSettings:RefreshExpiryInDays"] ?? ""));

    await _userManager.UpdateAsync(user);

    return new TokenDTO
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      ExpiryTime = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:AccessExpiryInMinutes"] ?? ""))
    };
  }


  public async Task<ApiResponse<TokenDTO>> Register(RegisterDTO registerInfo)
  {
    var existingUser = await _userManager.FindByEmailAsync(registerInfo.Email);
    if (existingUser != null)
    {
      return ApiResponse<TokenDTO>.ErrorResponse("User with this email already exists");
    }

    var user = new User
    {
      Email = registerInfo.Email,
      UserName = registerInfo.Email
    };

    var result = await _userManager.CreateAsync(user, registerInfo.Password);
    if (!result.Succeeded)
    {
      var errors = result.Errors.Select(e => e.Description).ToList();
      return ApiResponse<TokenDTO>.ErrorResponse("Registration failed", errors);
    }

    //var userDTO = new UserDTO
    //{
    //  Id = user.Id,
    //  Email = user.Email,
    //  CreatedAt = user.CreatedAt
    //};

    return ApiResponse<TokenDTO>.SuccessResponse(await CreateTokenDTO(user), "Registration successful");
  }


  public async Task<ApiResponse<TokenDTO>> Login(LoginDTO loginInfo)
  {
    try
    {
      var user = await _userManager.FindByEmailAsync(loginInfo.Email);
      if (user == null || !await _userManager.CheckPasswordAsync(user, loginInfo.Password))
      {
        return ApiResponse<TokenDTO>.ErrorResponse("Invalid email or password");
      }

      return ApiResponse<TokenDTO>.SuccessResponse(await CreateTokenDTO(user), "Login successful");
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.ToString());
      return ApiResponse<TokenDTO>.ErrorResponse("Server error when logging in");
    }
  }


  public async Task<ApiResponse<TokenDTO>> RefreshToken(RefreshTokenDTO refreshInfo)
  {
    try
    {
      var principal = _tokenServices.GetPrincipalFromExpiredToken(refreshInfo.AccessToken);
      var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      if (userId == null)
      {
        return ApiResponse<TokenDTO>.ErrorResponse("Invalid token");
      }

      var user = await _userManager.FindByIdAsync(userId);
      if (user == null || user.RefreshToken != refreshInfo.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
      {
        return ApiResponse<TokenDTO>.ErrorResponse("Invalid refresh token");
      }

      return ApiResponse<TokenDTO>.SuccessResponse(await CreateTokenDTO(user), "Token refreshed successfully");
    }
    catch (Exception)
    {
      return ApiResponse<TokenDTO>.ErrorResponse("Invalid token");
    }
  }


  public async Task<ApiResponse<bool>> Logout(string userId)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
      return ApiResponse<bool>.ErrorResponse("User not found");
    }

    user.RefreshToken = null;
    user.RefreshTokenExpiryTime = DateTime.UtcNow;

    await _userManager.UpdateAsync(user);

    return ApiResponse<bool>.SuccessResponse(true, "Logout successful");
  }
}
