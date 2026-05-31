using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services
{
    public interface IBookService
    {
        Task<Book?> AddBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);
        Task<List<BookDto>> GetAllBooksAsync();
        Task<BookDetailsDto?> GetBookByIdAsync(int id);
        Task<Book?> UpdateBookAsync(int id, Book book);
    }
}