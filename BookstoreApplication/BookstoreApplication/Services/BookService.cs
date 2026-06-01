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

        public BookService(IBookRepo bookRepo, IAuthorRepo authorRepo, IPublisherRepo publisherRepo, IMapper mapper)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _publisherRepo = publisherRepo;
            _mapper = mapper;
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
               throw new BadRequestException($"Author with ID {book.AuthorId} does not exist.");
            }

            var publisher = await _publisherRepo.GetPublisherByIdAsync(book.PublisherId);
            if (publisher == null)
            {
                throw new BadRequestException($"Publisher with ID {book.PublisherId} does not exist.");
            }
            return await _bookRepo.AddBookAsync(book);
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            var existingBook = await _bookRepo.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                throw new NotFoundException(id);
            }
            book.Id = id;
            book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);
            var author = await _authorRepo.GetAuthorByIdAsync(book.AuthorId);
            if (author == null)
            {
                throw new BadRequestException($"Author with ID {book.AuthorId} does not exist.");
            }
            var publisher = await _publisherRepo.GetPublisherByIdAsync(book.PublisherId);
            if (publisher == null)
            {
                throw new BadRequestException($"Publisher with ID {book.PublisherId} does not exist.");
            }

            existingBook.Title = book.Title;
            existingBook.PageCount = book.PageCount;
            existingBook.PublishedDate = book.PublishedDate;
            existingBook.ISBN = book.ISBN;
            existingBook.AuthorId = book.AuthorId;
            existingBook.PublisherId = book.PublisherId;

            return await _bookRepo.UpdateBookAsync(existingBook);
        }

        public async Task DeleteBookAsync(int id)
        {
            var success = await _bookRepo.DeleteBookAsync(id);
            if (!success)
            {
                throw new NotFoundException(id);
            }
        }
    }
}
