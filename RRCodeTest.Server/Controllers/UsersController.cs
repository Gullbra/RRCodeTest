using RRCodeTest.Server.Models.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRCodeTest.Server.Models;
using System.Security.Claims;

namespace RRCodeTest.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : Controller
{
  private readonly UserManager<User> _userManager;

  public UsersController(UserManager<User> userManager)
  {
    _userManager = userManager;
  }


  [HttpGet("profile")]
  public async Task<IActionResult> GetProfile()
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userId == null)
    {
      return Unauthorized();
    }

    var user = await _userManager.FindByIdAsync(userId);

    if (user == null)
    {
      return NotFound();
    }

    var userDTO = new UserDTO
    {
      Id = user.Id,
      Email = user.Email ?? "",
      CreatedAt = user.CreatedAt
    };

    var response = ApiResponse<UserDTO>.SuccessResponse(userDTO, "Profile retrieved successfully");
    return Ok(response);
  }
}
