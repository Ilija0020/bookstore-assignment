using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Utils;

namespace BookstoreApplication.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepo _authorRepo;
        private readonly IMapper _mapper;
        private const int PageSize = 10;

        public AuthorService(IAuthorRepo authorRepo, IMapper mapper)
        {
            _authorRepo = authorRepo;
            _mapper = mapper;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _authorRepo.GetAllAuthorsAsync();
        }

        public async Task<PaginatedList<AuthorDTO>> GetAllAuthorsPagedAsync(int page)
        {
            var authors = await _authorRepo.GetAllAuthorsPagedAsync(page);
            var dtos = authors.Items.Select(_mapper.Map<AuthorDTO>).ToList();

            return new PaginatedList<AuthorDTO>(dtos, authors.Count, authors.PageIndex, PageSize);
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
