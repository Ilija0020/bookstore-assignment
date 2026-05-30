using BookstoreApplication.Models;
using BookstoreApplication.Repositories;

namespace BookstoreApplication.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepo _authorRepo;

        public AuthorService(IAuthorRepo authorRepo)
        {
            _authorRepo = authorRepo;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _authorRepo.GetAllAuthorsAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await _authorRepo.GetAuthorByIdAsync(id);
        }

        public async Task<Author> AddAuthorAsync(Author author)
        {
            return await _authorRepo.AddAuthorAsync(author);
        }

        public async Task<Author?> UpdateAuthorAsync(int id, Author author)
        {
            if (id != author.Id)
            {
                return null;
            }
            var existingAuthor = await _authorRepo.GetAuthorByIdAsync(id);
            if (existingAuthor == null)
            {
                return null;
            }
            existingAuthor.FullName = author.FullName;
            existingAuthor.Biography = author.Biography;
            existingAuthor.DateOfBirth = author.DateOfBirth;

            return await _authorRepo.UpdateAuthorAsync(existingAuthor);
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            return await _authorRepo.DeleteAuthorAsync(id);
        }
    }
}
