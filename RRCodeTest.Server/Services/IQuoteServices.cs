using Microsoft.EntityFrameworkCore;
using RRCodeTest.Server.DB;
using RRCodeTest.Server.Models;
using RRCodeTest.Server.Models.DTOs.Quote;

namespace RRCodeTest.Server.Services;

public interface IQuoteServices
{
  Task<ApiResponse<IEnumerable<QuoteDTO>>> GetQuotes(string userId);
  Task<ApiResponse<QuoteDTO>> AddQuote(NewQuoteDTO quoteInfo, string userId, User user);
  Task<ApiResponse<QuoteDTO>> UpdateQuote(NewQuoteDTO updatedQuote, string quoteId, string userId);
  Task<ApiResponse<QuoteDTO>> DeleteQuote(string quoteId, string userId);
}


public class QuoteService(AppDbContext context) : IQuoteServices
{

  private readonly AppDbContext _context = context;


  public async Task<ApiResponse<IEnumerable<QuoteDTO>>> GetQuotes(string userId)
  {
    try
    {
      var quoteDTOs = (await _context.Quotes.Where(b => b.UserId == userId).Include(b => b.User).ToListAsync()).Select(b => new QuoteDTO
      {
        Id = b.Id,
        Text = b.Text,
        Author = b.Author,
        Source = b.Source,
        UserId = b.UserId,
      }).ToList();

      return ApiResponse<IEnumerable<QuoteDTO>>.SuccessResponse(quoteDTOs, $"Quotes for user {userId} retrieved");
    }
    catch (Exception ex)
    {
      return ApiResponse<IEnumerable<QuoteDTO>>.ErrorResponse("Could not fetch quotes from DB", [ex.Message]);
    }
  }


  public async Task<ApiResponse<QuoteDTO>> AddQuote(NewQuoteDTO quoteInfo, string userId, User user)
  {
    try
    {
      var result = await _context.Quotes.AddAsync(new Quote
      {
        Text = quoteInfo.Text,
        Author = quoteInfo.Author,
        Source = quoteInfo.Source,
        UserId = userId,
        User = user
      });
      await _context.SaveChangesAsync();

      return ApiResponse<QuoteDTO>.SuccessResponse(new QuoteDTO
      {
        Id = result.Entity.Id,
        Text = quoteInfo.Text,
        Author = quoteInfo.Author,
        Source = quoteInfo.Source,
        UserId = userId
      });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Full exception: {ex}");
      Console.WriteLine($"Inner exception: {ex.InnerException}");
      return ApiResponse<QuoteDTO>.ErrorResponse("Could not add the quote to the DB", [ex.Message]);
    }
  }


  public async Task<ApiResponse<QuoteDTO>> UpdateQuote(NewQuoteDTO updatedQuote, string quoteId, string userId)
  {
    try
    {
      var quote = await _context.Quotes
        .Where(b => b.UserId == userId && b.Id == int.Parse(quoteId))
        .SingleOrDefaultAsync();

      if (quote == null)
      {
        return ApiResponse<QuoteDTO>.ErrorResponse("Quote was not found", []);
      }

      quote.Text = updatedQuote.Text;
      quote.Author = updatedQuote.Author;
      quote.Source = updatedQuote.Source;

      await _context.SaveChangesAsync();

      return ApiResponse<QuoteDTO>.SuccessResponse(
        new QuoteDTO
        {
          Author = quote.Author,
          Text = quote.Text,
          Id = quote.Id,
          Source = quote.Source,
          UserId = userId
        },
         "Quote updated"
      );
    }
    catch (Exception ex)
    {
      return ApiResponse<QuoteDTO>.ErrorResponse("Could not update the quote", [ex.Message]);
    }
  }


  public async Task<ApiResponse<QuoteDTO>> DeleteQuote(string quoteId, string userId)
  {
    try
    {
      var quote = await _context.Quotes
        .Where(b => b.UserId == userId && b.Id == int.Parse(quoteId))
        .SingleOrDefaultAsync();

      if (quote == null)
      {
        return ApiResponse<QuoteDTO>.ErrorResponse("Quote was not found", []);
      }

      var returnObj = new QuoteDTO
      {
        Author = quote.Author,
        Text = quote.Text,
        Id = quote.Id,
        Source = quote.Source,
        UserId = userId
      };

      _context.Quotes.Remove(quote);
      await _context.SaveChangesAsync();

      return ApiResponse<QuoteDTO>.SuccessResponse(returnObj, "Successfully deleted quote");
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Full exception: {ex}");
      Console.WriteLine($"Inner exception: {ex.InnerException}");
      return ApiResponse<QuoteDTO>.ErrorResponse("Could not delete the quote", [ex.Message]);
    }
  }
}
