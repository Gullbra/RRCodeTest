using RRCodeTest.Server.Models;
using RRCodeTest.Server.Models.DTOs.Token;
using RRCodeTest.Server.Models.DTOs.User;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using RRCodeTest.Server.Models.DTOs.Quote;

namespace RRCodeTest.Server.Services;

public class AuthServices : IAuthServices
{
  private readonly UserManager<User> _userManager;
  private readonly ITokenServices _tokenServices;
  private readonly IConfiguration _configuration;
  private readonly IQuoteServices _quoteServices;


  public AuthServices(UserManager<User> userManager, ITokenServices tokenServices, IConfiguration configuration, IQuoteServices quoteServices)
  {
    _userManager = userManager;
    _tokenServices = tokenServices;
    _configuration = configuration;
    _quoteServices = quoteServices;
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

    var qouteInit = new List<NewQuoteDTO>() 
    { 
      new () {
        Text = "The only way to do great work is to love what you do.", 
        Author = "Steve Jobs"
      },
      new () {
        Text = "For a moment, nothing happened. Then, after a second or so, nothing continued to happen.", 
        Author = "Douglas Adams",
        Source = "The Restaurant at the End of the Universe"
      },
      new () {
        Text = "I am an optimist. It does not seem too much use being anything else.", 
        Author = "Winstone Churchill"
      },
      new () {
        Text = "je pense, donc je suis.", 
        Author = "René Descartes"
      },
      new () {
        Text = "There is a crack in everything.\r\nThat's how the light gets in.", 
        Author = "Leonard Cohen",
        Source = "Selected Poems"
      },
    };

    foreach(var quote in qouteInit) 
    {
      await _quoteServices.AddQuote(quote, user.Id, user);
    }

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
