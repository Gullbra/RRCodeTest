using api.DB;
using api.Models;
using api.Models.DTOs;
using api.Models.DTOs.Book;
using api.Models.DTOs.User;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : Controller
{
  private readonly AppDbContext _context;
  private readonly UserManager<User> _userManager;
  private readonly IBookServices _bookServices;


  public BooksController(AppDbContext context, UserManager<User> userManager, IBookServices bookServices)
  {
    _context = context;
    _userManager = userManager;
    _bookServices = bookServices;
  }


  [HttpGet]
  public async Task<IActionResult> GetBooks()
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userId == null)
    {
      return Unauthorized();
    }

    var result = await _bookServices.GetBooks(userId);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }


  [HttpPost]
  public async Task<ActionResult<BookDTO>> CreateBook([FromBody] NewBookDTO bookInfo)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

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


    var result = await _bookServices.AddBook(bookInfo, userId, user);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }


  [HttpPut("{id}")]
  public async Task<ActionResult<BookDTO>> UpdateBook(string id, NewBookDTO updatedBook)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

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

    var result = await _bookServices.UpdateBook(updatedBook, id, userId);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }


  [HttpDelete("{id}")]
  public async Task<ActionResult<BookDTO>> DeleteBook(string id)
  {
    if (!ModelState.IsValid)
    {
      return BadRequest(ModelState);
    }

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

    var result = await _bookServices.DeleteBook(id, userId);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }
}

