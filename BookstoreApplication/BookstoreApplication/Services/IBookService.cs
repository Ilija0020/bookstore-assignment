using BookstoreApplication.Models;

namespace BookstoreApplication.Services
{
    public interface IBookService
    {
        Task<Book?> AddBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book?> UpdateBookAsync(int id, Book book);
    }
}