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
        public IActionResult GetAll()
        {
            return Ok(_bookRepo.GetAllBooks());
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var book = _bookRepo.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public IActionResult Post(Book book)
        {
            // kreiranje knjige je moguće ako je izabran postojeći autor
            var author = _authorRepo.GetAuthorById(book.AuthorId);
            if (author == null)
            {
                return BadRequest();
            }

            // kreiranje knjige je moguće ako je izabran postojeći izdavač
            var publisher = _publisherRepo.GetPublisherById(book.PublisherId);
            if (publisher == null)
            {
                return BadRequest();
            }

            Book createdBook = _bookRepo.AddBook(book);
            return Ok(createdBook);
        }

        // PUT api/books/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            // izmena knjige je moguca ako je izabran postojeći autor
            var author = _authorRepo.GetAuthorById(book.AuthorId);
            if (author == null)
            {
                return BadRequest();
            }

            // izmena knjige je moguca ako je izabran postojeći izdavač
            var publisher = _publisherRepo.GetPublisherById(book.PublisherId);
            if (publisher == null)
            {
                return BadRequest();
            }

            Book updatedBook = _bookRepo.UpdateBook(book);
            return Ok(book);
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _bookRepo.DeleteBook(id);
            if (!book)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
