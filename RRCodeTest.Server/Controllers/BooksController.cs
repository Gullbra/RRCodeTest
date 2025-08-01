﻿using RRCodeTest.Server.DB;
using RRCodeTest.Server.Models.DTOs.Book;
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
public class BooksController : Controller
{
  private readonly UserManager<User> _userManager;
  private readonly IBookServices _bookServices;


  public BooksController(UserManager<User> userManager, IBookServices bookServices)
  {
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
  public async Task<ActionResult<BookDTO>> UpdateBook(string id, [FromBody]NewBookDTO updatedBook)
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

