using RRCodeTest.Server.Models;
using RRCodeTest.Server.Models.DTOs.Book;
using Microsoft.AspNetCore.Identity;
using RRCodeTest.Server.DB;
using Microsoft.EntityFrameworkCore;

namespace RRCodeTest.Server.Services;

public class BookServices(UserManager<User> userManager, AppDbContext context) : IBookServices
{
  private readonly UserManager<User> _userManager = userManager;
  private readonly AppDbContext _context = context;

  public async Task<ApiResponse<IEnumerable<BookDTO>>> GetBooks(string userId)
  {
    try
    {
      var bookDTOs = (await _context.Books.Where(b => b.UserId == userId).Include(b => b.User).ToListAsync()).Select(b => new BookDTO
      {
        Id = b.Id,
        Title = b.Title,
        Author = b.Author,
        DateOfPublication = b.DateOfPublication,
        UserId = b.UserId,
      }).ToList();

      return ApiResponse<IEnumerable<BookDTO>>.SuccessResponse(bookDTOs, $"Books for user {userId} retrieved");
    }
    catch (Exception ex)
    {
      return ApiResponse<IEnumerable<BookDTO>>.ErrorResponse("Could not fetch books from DB", [ex.Message]);
    }
  }


  public async Task<ApiResponse<BookDTO>> AddBook(NewBookDTO bookInfo, string userId, User user)
  {
    try
    {
      var result = await _context.Books.AddAsync(new Book
      {
        Title = bookInfo.Title,
        Author = bookInfo.Author,
        DateOfPublication = bookInfo.DateOfPublication,
        UserId = userId,
        User = user
      });
      await _context.SaveChangesAsync();

      return ApiResponse<BookDTO>.SuccessResponse(new BookDTO
      {
        Id = result.Entity.Id,
        Title = bookInfo.Title,
        Author = bookInfo.Author,
        DateOfPublication = bookInfo.DateOfPublication,
        UserId = userId
      });
    }
    catch (Exception ex)
    {
      //Console.WriteLine($"Full exception: {ex}");
      //Console.WriteLine($"Inner exception: {ex.InnerException}");
      return ApiResponse<BookDTO>.ErrorResponse("Could not add the book to the DB", [ex.Message]);
    }
  }


  public async Task<ApiResponse<BookDTO>> UpdateBook(NewBookDTO updatedBook, string bookId, string userId)
  {
    try
    {
      var book = await _context.Books
        .Where(b => b.UserId == userId && b.Id == int.Parse(bookId))
        .SingleOrDefaultAsync();

      if (book == null)
      {
        return ApiResponse<BookDTO>.ErrorResponse("Book was not found", []);
      }

      book.Title = updatedBook.Title;
      book.Author = updatedBook.Author;
      book.DateOfPublication = updatedBook.DateOfPublication;

      await _context.SaveChangesAsync();

      return ApiResponse<BookDTO>.SuccessResponse(
        new BookDTO
        {
          Author = book.Author,
          Title = book.Title,
          Id = book.Id,
          DateOfPublication= book.DateOfPublication,
          UserId = userId
        },
         "Book updated"
      );
    }
    catch (Exception ex)
    {
      //Console.WriteLine($"Full exception: {ex}");
      //Console.WriteLine($"Inner exception: {ex.InnerException}");
      return ApiResponse<BookDTO>.ErrorResponse("Could not update the book", [ex.Message]);
    }
  }


  public async Task<ApiResponse<BookDTO>> DeleteBook(string bookId, string userId)
  {
    try
    {
      var book = await _context.Books
        .Where(b => b.UserId == userId && b.Id == int.Parse(bookId))
        .SingleOrDefaultAsync();

      if (book == null)
      {
        return ApiResponse<BookDTO>.ErrorResponse("Book was not found", []);
      }

      var returnObj = new BookDTO
      {
        Author = book.Author,
        Title = book.Title,
        Id = book.Id,
        DateOfPublication = book.DateOfPublication,
        UserId = userId
      };

      _context.Books.Remove(book);
      await _context.SaveChangesAsync();

      return ApiResponse<BookDTO>.SuccessResponse(returnObj, "Successfully deleted book");
    }
    catch (Exception ex)
    {
      //Console.WriteLine($"Full exception: {ex}");
      //Console.WriteLine($"Inner exception: {ex.InnerException}");
      return ApiResponse<BookDTO>.ErrorResponse("Could not delete the book", [ex.Message]);
    }
  }
}
