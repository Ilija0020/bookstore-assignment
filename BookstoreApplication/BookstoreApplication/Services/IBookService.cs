using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Utils;

namespace BookstoreApplication.Services
{
    public interface IBookService
    {
        Task<Book> AddBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task<List<BookDto>> GetAllBooksAsync();
        Task<BookDetailsDto?> GetBookByIdAsync(int id);
        Task<Book> UpdateBookAsync(int id, Book book);
        Task<IEnumerable<BookDetailsDto>> GetAllSortedAsync(int sortType);
        List<SortTypeOption> GetSortTypes();
        Task<IEnumerable<BookDetailsDto>> GetAllFilteredAndSortedAsync(BookFilter filter, int sortType);
    }
}