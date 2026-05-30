using BookstoreApplication.Models;

namespace BookstoreApplication.Services
{
    public interface IAuthorService
    {
        Task<Author> AddAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(int id);
        Task<List<Author>> GetAllAuthorsAsync();
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<Author?> UpdateAuthorAsync(int id, Author author);
    }
}