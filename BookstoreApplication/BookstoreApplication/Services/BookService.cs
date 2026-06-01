using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;

namespace BookstoreApplication.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly IPublisherRepo _publisherRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepo bookRepo, IAuthorRepo authorRepo, IPublisherRepo publisherRepo, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _publisherRepo = publisherRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<BookDto>> GetAllBooksAsync()
        {
            var books = await _bookRepo.GetAllBooksAsync();
            return books
                .Select(book => _mapper.Map<BookDto>(book))
                .ToList();
        }

        public async Task<BookDetailsDto?> GetBookByIdAsync(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Book with ID {BookId} not found.", id);
                throw new NotFoundException(id);
            }
            return _mapper.Map<BookDetailsDto>(book);
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);

            var author = await _authorRepo.GetAuthorByIdAsync(book.AuthorId);
            if (author == null)
            {
                _logger.LogWarning("Failed to create book. Author with ID {AuthorId} does not exist.", book.AuthorId);
               throw new BadRequestException($"Author with ID {book.AuthorId} does not exist.");
            }

            var publisher = await _publisherRepo.GetPublisherByIdAsync(book.PublisherId);
            if (publisher == null)
            {
                _logger.LogWarning("Failed to create book. Publisher with ID {PublisherId} does not exist.", book.PublisherId);
                throw new BadRequestException($"Publisher with ID {book.PublisherId} does not exist.");
            }
            var createdBook = await _bookRepo.AddBookAsync(book);
            _logger.LogInformation("Book '{BookTitle}' created successfully with ID {BookId}.", createdBook.Title, createdBook.Id);
            return createdBook;
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            var existingBook = await _bookRepo.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                _logger.LogWarning("Failed to update book. Book with ID {BookId} was not found.", id);
                throw new NotFoundException(id);
            }
            book.Id = id;
            book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);
            var author = await _authorRepo.GetAuthorByIdAsync(book.AuthorId);
            if (author == null)
            {
                _logger.LogWarning("Failed to update book. Author with ID {AuthorId} does not exist.", book.AuthorId);
                throw new BadRequestException($"Author with ID {book.AuthorId} does not exist.");
            }
            var publisher = await _publisherRepo.GetPublisherByIdAsync(book.PublisherId);
            if (publisher == null)
            {
                _logger.LogWarning("Failed to update book. Publisher with ID {PublisherId} does not exist.", book.PublisherId);
                throw new BadRequestException($"Publisher with ID {book.PublisherId} does not exist.");
            }

            existingBook.Title = book.Title;
            existingBook.PageCount = book.PageCount;
            existingBook.PublishedDate = book.PublishedDate;
            existingBook.ISBN = book.ISBN;
            existingBook.AuthorId = book.AuthorId;
            existingBook.PublisherId = book.PublisherId;

            var updatedBook = await _bookRepo.UpdateBookAsync(existingBook);
            _logger.LogInformation("Book with ID {BookId} updated successfully.", updatedBook.Id);
            return updatedBook;
        }

        public async Task DeleteBookAsync(int id)
        {
            var success = await _bookRepo.DeleteBookAsync(id);
            if (!success)
            {
                _logger.LogWarning("Failed to delete book. Book with ID {BookId} was not found.", id);
                throw new NotFoundException(id);
            }
            _logger.LogInformation("Book with ID {BookId} deleted successfully.", id);
        }
    }
}
