using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using BookstoreApplication.Utils;
using System.Threading.Tasks;

namespace BookstoreApplication.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepo _bookRepo;
        private readonly IAuthorRepo _authorRepo;
        private readonly IPublisherRepo _publisherRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IBookRepo bookRepo, IAuthorRepo authorRepo, IPublisherRepo publisherRepo, IMapper mapper, ILogger<BookService> logger, IUnitOfWork unitOfWork)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _publisherRepo = publisherRepo;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
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

        public async Task<Book> AddBookAsync(SaveBookDTO bookDto)
        {
            var author = await _authorRepo.GetAuthorByIdAsync(bookDto.AuthorId);
            if (author == null)
            {
                _logger.LogWarning("Failed to create book. Author with ID {AuthorId} does not exist.", bookDto.AuthorId);
                throw new BadRequestException($"Author with ID {bookDto.AuthorId} does not exist.");
            }

            var publisher = await _publisherRepo.GetPublisherByIdAsync(bookDto.PublisherId);
            if (publisher == null)
            {
                _logger.LogWarning("Failed to create book. Publisher with ID {PublisherId} does not exist.", bookDto.PublisherId);
                throw new BadRequestException($"Publisher with ID {bookDto.PublisherId} does not exist.");
            }

            var book = _mapper.Map<Book>(bookDto);
            book.PublishedDate = DateTime.SpecifyKind(bookDto.PublishedDate, DateTimeKind.Utc);
            book.AverageRating = 0;

            var createdBook = await _bookRepo.AddBookAsync(book);
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Book '{BookTitle}' created successfully with ID {BookId}.", createdBook.Title, createdBook.Id);
            return createdBook;
        }

        public async Task<Book> UpdateBookAsync(int id, SaveBookDTO bookDto)
        {
            var existingBook = await _bookRepo.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                _logger.LogWarning("Failed to update book. Book with ID {BookId} was not found.", id);
                throw new NotFoundException(id);
            }

            var author = await _authorRepo.GetAuthorByIdAsync(bookDto.AuthorId);
            if (author == null)
            {
                _logger.LogWarning("Failed to update book. Author with ID {AuthorId} does not exist.", bookDto.AuthorId);
                throw new BadRequestException($"Author with ID {bookDto.AuthorId} does not exist.");
            }

            var publisher = await _publisherRepo.GetPublisherByIdAsync(bookDto.PublisherId);
            if (publisher == null)
            {
                _logger.LogWarning("Failed to update book. Publisher with ID {PublisherId} does not exist.", bookDto.PublisherId);
                throw new BadRequestException($"Publisher with ID {bookDto.PublisherId} does not exist.");
            }

            _mapper.Map(bookDto, existingBook);

            existingBook.PublishedDate = DateTime.SpecifyKind(existingBook.PublishedDate, DateTimeKind.Utc);

            var updatedBook = await _bookRepo.UpdateBookAsync(existingBook);
            await _unitOfWork.SaveAsync();
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
            await _unitOfWork.SaveAsync();
            _logger.LogInformation("Book with ID {BookId} deleted successfully.", id);
        }

        public async Task<IEnumerable<BookDetailsDto>> GetAllSortedAsync(int sortType)
        {
            var books = await _bookRepo.GetAllSortedAsync(sortType);

            return _mapper.Map<IEnumerable<BookDetailsDto>>(books);
        }

        public List<SortTypeOption> GetSortTypes()
        {
            return _bookRepo.GetSortTypes();
        }

        public async Task<IEnumerable<BookDetailsDto>> GetAllFilteredAndSortedAsync(BookFilter filter, int sortType)
        {
            var books = await _bookRepo.GetAllFilteredAndSortedAsync(filter, sortType);
            var dtos = books.Select(_mapper.Map<BookDetailsDto>).ToList();
            return dtos;
        }
    }
}
