using RRCodeTest.Server.Models.DTOs.Token;
using RRCodeTest.Server.Models.DTOs.User;
using RRCodeTest.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RRCodeTest.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
  private readonly IAuthServices _authServices;


  public AuthController(IAuthServices authServices)
  {
    _authServices = authServices;
  }


  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterDTO registerInfo)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var result = await _authServices.Register(registerInfo);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }



  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var result = await _authServices.Login(loginDto);

    if (result.Success)
    {
      return Ok(result);
    }
    return Unauthorized(result);
  }


  [HttpPost("refresh")]
  public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDto)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

    var result = await _authServices.RefreshToken(refreshTokenDto);

    if (result.Success)
    {
      return Ok(result);
    }

    return Unauthorized(result);
  }


  [HttpPost("logout")]
  [Authorize]
  public async Task<IActionResult> Logout()
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userId == null)
    {
      return Unauthorized();
    }

    var result = await _authServices.Logout(userId);

    if (result.Success)
    {
      return Ok(result);
    }

    return BadRequest(result);
  }
}

