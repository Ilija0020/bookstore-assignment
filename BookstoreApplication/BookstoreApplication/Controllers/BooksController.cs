using BookstoreApplication.Data;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookRepo _bookRepo;
        private readonly AuthorRepo _authorRepo;
        private readonly PublisherRepo _publisherRepo;

        public BooksController(BookRepo bookRepo, AuthorRepo authorRepo, PublisherRepo publisherRepo)
        {
            _bookRepo = bookRepo;
            _authorRepo = authorRepo;
            _publisherRepo = publisherRepo;
        }
        // GET: api/books
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _bookRepo.GetAllBooksAsync());
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public async Task<IActionResult> PostAsync(Book book)
        {
            // kreiranje knjige je moguće ako je izabran postojeći autor
            var author = await _authorRepo.GetAuthorByIdAsync(book.AuthorId);
            if (author == null)
            {
                return BadRequest();
            }

            // kreiranje knjige je moguće ako je izabran postojeći izdavač
            var publisher = await _publisherRepo.GetPublisherByIdAsync(book.PublisherId);
            if (publisher == null)
            {
                return BadRequest();
            }

            Book createdBook = await _bookRepo.AddBookAsync(book);
            return Ok(createdBook);
        }

        // PUT api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            var existingBook = await _bookRepo.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            // izmena knjige je moguca ako je izabran postojeći autor
            var author = await _authorRepo.GetAuthorByIdAsync(book.AuthorId);
            if (author == null)
            {
                return BadRequest();
            }

            // izmena knjige je moguca ako je izabran postojeći izdavač
            var publisher = await _publisherRepo.GetPublisherByIdAsync(book.PublisherId);
            if (publisher == null)
            {
                return BadRequest();
            }

            existingBook.Title = book.Title;
            existingBook.PageCount = book.PageCount;
            existingBook.PublishedDate = book.PublishedDate;
            existingBook.ISBN = book.ISBN;
            existingBook.AuthorId = book.AuthorId;
            existingBook.PublisherId = book.PublisherId;

            await _bookRepo.UpdateBookAsync(existingBook);
            return Ok(existingBook);
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _bookRepo.DeleteBookAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
