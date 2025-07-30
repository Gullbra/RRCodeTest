using RRCodeTest.Server.DB;
using RRCodeTest.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRCodeTest.Server.Models;

namespace RRCodeTest.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevController : Controller
{
  private readonly AppDbContext _context;
  private readonly UserManager<User> _userManager;
  private readonly IBookServices _bookServices;

  public DevController(AppDbContext context, UserManager<User> userManager, IBookServices bookServices)
  {
    _context = context;
    _userManager = userManager;
    _bookServices = bookServices;
  }


  [HttpGet("Users")]
  public async Task<IActionResult> DevUsers()
  {
    try
    {
      var users = await _context.Users
          .Select(u => new
          {
            u.Id,
            u.Email,
            u.CreatedAt,
            u.RefreshToken
          })
          .ToListAsync();

      return Ok(new
      {
        Success = true,
        Data = users,
        Count = users.Count,
        Message = "Users retrieved successfully"
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new
      {
        Success = false,
        Message = "Failed to retrieve users",
        Error = ex.Message
      });
    }
  }


  [HttpGet("Books")]
  public async Task<IActionResult> DevBooks()
  {
    try
    {
      var books = await _context.Books
          //.Include(b => b.User)
          .Select(b => new
          {
            b.Id,
            b.Title,
            b.Author,
            b.DateOfPublication,
            b.UserId,
            //UserEmail = b.User.Email
          })
          .ToListAsync();

      return Ok(new
      {
        Success = true,
        Data = books,
        Count = books.Count,
        Message = "Books retrieved successfully"
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new
      {
        Success = false,
        Message = "Failed to retrieve books",
        Error = ex.Message
      });
    }
  }


  [HttpGet("Quotes")]
  public async Task<IActionResult> DevQuotes()
  {
    try
    {
      var quotes = await _context.Quotes
          //.Include(b => b.User)
          .Select(b => new
          {
            b.Id,
            b.Text,
            b.Author,
            b.Source,
            b.UserId,
            //UserEmail = b.User.Email
          })
          .ToListAsync();

      return Ok(new
      {
        Success = true,
        Data = quotes,
        Count = quotes.Count,
        Message = "Quotes retrieved successfully"
      });
    }
    catch (Exception ex)
    {
      return StatusCode(500, new
      {
        Success = false,
        Message = "Failed to retrieve quotes",
        Error = ex.Message
      });
    }
  }
}
