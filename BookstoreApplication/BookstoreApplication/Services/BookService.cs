using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using BookstoreApplication.Services.DTOs;

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
                return null;
            }
            return _mapper.Map<BookDetailsDto>(book);
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
