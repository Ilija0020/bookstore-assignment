using BookstoreApplication.Domain.Common;
using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Queries;

namespace BookstoreApplication.Domain.Repositories
{
    public interface IBookRepo
    {
        Task<Book> AddBookAsync(Book book);
        Task<bool> DeleteBookAsync(int id);
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<Book> UpdateBookAsync(Book book);
        Task<IEnumerable<Book>> GetAllSortedAsync(int sortType);
        List<SortTypeOption> GetSortTypes();
        Task<IEnumerable<Book>> GetAllFilteredAndSortedAsync(BookFilter filter, int sortType);
    }
}
