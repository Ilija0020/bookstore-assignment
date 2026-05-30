using BookstoreApplication.Models;
using BookstoreApplication.Repositories;

namespace BookstoreApplication.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly IPublisherRepo _publisherRepo;

        public BookService(IBookRepo bookRepo, IAuthorRepo authorRepo, IPublisherRepo publisherRepo)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _publisherRepo = publisherRepo;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _bookRepo.GetAllBooksAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _bookRepo.GetBookByIdAsync(id);
        }

        public async Task<Book?> AddBookAsync(Book book)
        {
            book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);

            var author = await _authorRepo.GetAuthorByIdAsync(book.AuthorId);
            if (author == null)
            {
                return null;
            }

            var publisher = await _publisherRepo.GetPublisherByIdAsync(book.PublisherId);
            if (publisher == null)
            {
                return null;
            }
            return await _bookRepo.AddBookAsync(book);
        }

        public async Task<Book?> UpdateBookAsync(int id, Book book)
        {
            var existingBook = await _bookRepo.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return null;
            }
            book.Id = id;
            book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);
            var author = await _authorRepo.GetAuthorByIdAsync(book.AuthorId);
            if (author == null)
            {
                return null;
            }
            var publisher = await _publisherRepo.GetPublisherByIdAsync(book.PublisherId);
            if (publisher == null)
            {
                return null;
            }

            existingBook.Title = book.Title;
            existingBook.PageCount = book.PageCount;
            existingBook.PublishedDate = book.PublishedDate;
            existingBook.ISBN = book.ISBN;
            existingBook.AuthorId = book.AuthorId;
            existingBook.PublisherId = book.PublisherId;

            return await _bookRepo.UpdateBookAsync(existingBook);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await _bookRepo.DeleteBookAsync(id);
        }
    }
}
