using BookstoreApplication.Data;
using BookstoreApplication.Models;
using BookstoreApplication.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories
{
    public class AuthorRepo : IAuthorRepo
    {
        private readonly AppDbContext _context;
        private const int PageSize = 10;

        public AuthorRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<PaginatedList<Author>> GetAllAuthorsPagedAsync(int page)
        {
            IQueryable<Author> authors = _context.Authors;

            int pageIndex = page - 1;
            var count = await authors.CountAsync();
            var items = await authors.Skip(pageIndex * PageSize).Take(PageSize).ToListAsync();
            
            PaginatedList<Author> result = new PaginatedList<Author>(items, count, pageIndex, PageSize);
            return result;
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Author> AddAuthorAsync(Author author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
            return author;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            Author? author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return false;
            }
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
