using BookstoreApplication.Data;
using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories
{
    public class AuthorRepo
    {
        private AppDbContext _context;

        public AuthorRepo(AppDbContext context)
        {
            _context = context;
        }

        public List<Author> GetAllAuthors()
        {
            return _context.Authors.ToList();
        }

        public Author? GetAuthorById(int id)
        {
            return _context.Authors.FirstOrDefault(a => a.Id == id);
        }

        public Author AddAuthor(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();
            return author;
        }

        public Author UpdateAuthor(Author author)
        {
            _context.Authors.Update(author);
            _context.SaveChanges();
            return author;
        }

        public bool DeleteAuthor(int id)
        {
            Author? author = _context.Authors.Find(id);
            if (author == null)
            {
                return false;
            }
            _context.Authors.Remove(author);
            _context.SaveChanges();
            return true;
        }
    }
}
