using api.Models;
using api.Models.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api.Controllers;

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
      Console.WriteLine(userId);
      return NotFound();
    }

    var userDTO = new UserDTO
    {
      Id = user.Id,
      Email = user.Email,
      CreatedAt = user.CreatedAt
    };

    var response = ApiResponse<UserDTO>.SuccessResponse(userDTO, "Profile retrieved successfully");
    return Ok(response);
  }
}
