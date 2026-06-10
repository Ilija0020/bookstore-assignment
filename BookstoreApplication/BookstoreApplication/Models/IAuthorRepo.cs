using BookstoreApplication.Utils;

namespace BookstoreApplication.Models
{
    public interface IAuthorRepo
    {
        Task<Author> AddAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(int id);
        Task<List<Author>> GetAllAuthorsAsync();
        Task<PaginatedList<Author>> GetAllAuthorsPagedAsync(int page);
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<Author> UpdateAuthorAsync(Author author);
    }
}