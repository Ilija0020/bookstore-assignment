using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Utils;

namespace BookstoreApplication.Services
{
    public interface IAuthorService
    {
        Task<Author> AddAuthorAsync(Author author);
        Task<bool> DeleteAuthorAsync(int id);
        Task<List<Author>> GetAllAuthorsAsync();
        Task<PaginatedList<AuthorDTO>> GetAllAuthorsPagedAsync(int page);
        Task<Author?> GetAuthorByIdAsync(int id);
        Task<Author?> UpdateAuthorAsync(int id, Author author);
    }
}