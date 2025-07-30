using RRCodeTest.Server.DB;
using RRCodeTest.Server.Models.DTOs.Quote;
using RRCodeTest.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RRCodeTest.Server.Models;
using System.Security.Claims;

namespace RRCodeTest.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuotesController : Controller
{
  private readonly UserManager<User> _userManager;
  private readonly IQuoteServices _quoteServices;


  public QuotesController(UserManager<User> userManager, IQuoteServices quoteServices)
  {
    _userManager = userManager;
    _quoteServices = quoteServices;
  }


  [HttpGet]
  public async Task<IActionResult> GetQuotes()
  {
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userId == null)
    {
      return Unauthorized();
    }

    var result = await _quoteServices.GetQuotes(userId);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }


  [HttpPost]
  public async Task<ActionResult<QuoteDTO>> CreateQuote([FromBody] NewQuoteDTO quoteInfo)
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


    var result = await _quoteServices.AddQuote(quoteInfo, userId, user);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }


  [HttpPut("{id}")]
  public async Task<ActionResult<QuoteDTO>> UpdateQuote(string id, [FromBody] NewQuoteDTO updatedQuote)
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

    var result = await _quoteServices.UpdateQuote(updatedQuote, id, userId);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }


  [HttpDelete("{id}")]
  public async Task<ActionResult<QuoteDTO>> DeleteQuote(string id)
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

    var result = await _quoteServices.DeleteQuote(id, userId);

    if (result.Success)
    {
      return Ok(result);
    }
    return BadRequest(result);
  }
}
