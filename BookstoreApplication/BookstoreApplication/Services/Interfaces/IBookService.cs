using BookstoreApplication.Domain.Common;
using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Queries;
using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IBookService
    {
        Task<Book> AddBookAsync(SaveBookDTO bookDto);
        Task DeleteBookAsync(int id);
        Task<List<BookDto>> GetAllBooksAsync();
        Task<BookDetailsDto?> GetBookByIdAsync(int id);
        Task<Book> UpdateBookAsync(int id, SaveBookDTO bookDto);
        Task<IEnumerable<BookDetailsDto>> GetAllSortedAsync(int sortType);
        List<SortTypeOption> GetSortTypes();
        Task<IEnumerable<BookDetailsDto>> GetAllFilteredAndSortedAsync(BookFilter filter, int sortType);
    }
}
