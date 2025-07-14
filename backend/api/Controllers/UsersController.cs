using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class UsersController : Controller
{
  private readonly UserManager<User> _userManager;

  public UsersController(UserManager<User> userManager)
  {
    _userManager = userManager;
  }



  [HttpGet]
  //public async Task<IActionResult> GetProfile()
  public string GetProfile()
  {
    return "Hey";
  }



//  [HttpPut("profile")]
//  public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateProfileDto)
//  {
//    if (!ModelState.IsValid)
//    {
//      return BadRequest(ModelState);
//    }

//    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//    if (userId == null)
//    {
//      return Unauthorized();
//    }

//    var user = await _userManager.FindByIdAsync(userId);

//    if (user == null)
//    {
//      return NotFound();
//    }

//    user.FirstName = updateProfileDto.FirstName;
//    user.LastName = updateProfileDto.LastName;

//    var result = await _userManager.UpdateAsync(user);

//    if (!result.Succeeded)
//    {
//      var errors = result.Errors.Select(e => e.Description).ToList();
//      var errorResponse = ApiResponse<UserDto>.ErrorResponse("Update failed", errors);
//      return BadRequest(errorResponse);
//    }

//    var userDto = new UserDto
//    {
//      Id = user.Id,
//      FirstName = user.FirstName,
//      LastName = user.LastName,
//      Email = user.Email,
//      CreatedAt = user.CreatedAt
//    };

//    var response = ApiResponse<UserDto>.SuccessResponse(userDto, "Profile updated successfully");
//    return Ok(response);
//  }
}
