﻿using RRCodeTest.Server.Models;
using RRCodeTest.Server.Models.DTOs.Book;

namespace RRCodeTest.Server.Services;

public interface IBookServices
{
  Task<ApiResponse<IEnumerable<BookDTO>>> GetBooks(string userId);
  Task<ApiResponse<BookDTO>> AddBook(NewBookDTO bookInfo, string userId, User user);
  Task<ApiResponse<BookDTO>> UpdateBook(NewBookDTO updatedBook, string bookId, string userId);
  Task<ApiResponse<BookDTO>> DeleteBook(string bookId, string userId);
}
