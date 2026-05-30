using BookstoreApplication.Data;
using BookstoreApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories
{
    public class AuthorRepo : IAuthorRepo
    {
        private AppDbContext _context;

        public AuthorRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
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
