using BookstoreApplication.Domain.Common;
using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services.Interfaces
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
