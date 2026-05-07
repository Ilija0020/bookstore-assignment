using BookstoreApplication.Data;
using BookstoreApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories
{
    public class BookRepo
    {

        private AppDbContext _context;
        public BookRepo(AppDbContext context)
        {
            _context = context;
        }

        public List<Book> GetAllBooks()
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToList();
                
        }

        public Book? GetBookById(int id)
        {
            return _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefault(b => b.Id == id);
        }

        public Book AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book;
        }

        public Book UpdateBook(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
            return book;
        }

        public bool DeleteBook(int id)
        {
            Book? book = _context.Books.Find(id);
            if (book == null)
            {
                return false;
            }
            _context.Books.Remove(book);
            _context.SaveChanges();
            return true;
        }
    }
}
