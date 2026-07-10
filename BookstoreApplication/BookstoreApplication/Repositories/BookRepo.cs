using BookstoreApplication.Data;
using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories
{
    public class BookRepo : IBookRepo
    {

        private AppDbContext _context;
        public BookRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .ToListAsync();

        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public Task<Book> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            return Task.FromResult(book);
        }

        public Task<Book> UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            return Task.FromResult(book);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            Book? book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }
            _context.Books.Remove(book);
            return true;
        }

        public List<SortTypeOption> GetSortTypes()
        {
            List<SortTypeOption> options = new List<SortTypeOption>();
            var enumValues = Enum.GetValues(typeof(BookSortType));
            foreach (BookSortType sortType in enumValues)
            {
                options.Add(new SortTypeOption(sortType));
            }
            return options;
        }

        public async Task<IEnumerable<Book>> GetAllSortedAsync(int sortType)
        {
            IQueryable<Book> books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher);

            books = SortBooks(books, sortType);
            return await books.ToListAsync();
        }

        private static IQueryable<Book> SortBooks(IQueryable<Book> books, int sortType)
        {
            return sortType switch
            {
                (int)BookSortType.TITLE_ASCENDING => books.OrderBy(b => b.Title),
                (int)BookSortType.TITLE_DESCENDING => books.OrderByDescending(b => b.Title),
                (int)BookSortType.PUBLISHED_DATE_ASCENDING => books.OrderBy(b => b.PublishedDate),
                (int)BookSortType.PUBLISHED_DATE_DESCENDING => books.OrderByDescending(b => b.PublishedDate),
                (int)BookSortType.AUTHOR_NAME_ASCENDING => books.OrderBy(b => b.Author!.FullName),
                (int)BookSortType.AUTHOR_NAME_DESCENDING => books.OrderByDescending(b => b.Author!.FullName),
                _ => books.OrderBy(b => b.Title)
            };
        }

        public async Task<IEnumerable<Book>> GetAllFilteredAndSortedAsync(BookFilter filter, int sortType)
        {
            IQueryable<Book> books = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher);
            
            books = FilterBooks(books, filter);
            books = SortBooks(books, sortType);

            return await books.ToListAsync();
        }

        private IQueryable<Book> FilterBooks(IQueryable<Book> books, BookFilter filter)
        {
            if (!string.IsNullOrEmpty(filter.Title))
            {
                books = books.Where(b =>
                b.Title.ToLower().Contains(filter.Title.ToLower()));
            }
            if (filter.PublishedDateFrom != null)
            {
                books = books.Where(b => b.PublishedDate >=
                filter.PublishedDateFrom);
            }
            if (filter.PublishedDateTo != null)
            {
                books = books.Where(b => b.PublishedDate <=
                filter.PublishedDateTo);
            }
            if (!string.IsNullOrEmpty(filter.AuthorFullName))
            {
                books = books.Where(b =>
                b.Author!.FullName.ToLower().Contains(filter.AuthorFullName.ToLower()));
            }
            if (filter.AuthorId != null)
            {
                books = books.Where(b => b.AuthorId == filter.AuthorId);
            }
            if (filter.AuthorDateOfBirthFrom != null)
            {
                books = books.Where(b => b.Author!.DateOfBirth >=
                filter.AuthorDateOfBirthFrom);
            }
            if (filter.AuthorDateOfBirthTo != null)
            {
                books = books.Where(b => b.Author!.DateOfBirth <=
                filter.AuthorDateOfBirthTo);
            }
            return books;
        }
    }
}
