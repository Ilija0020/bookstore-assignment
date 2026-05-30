using BookstoreApplication.Data;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using BookstoreApplication.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService; 
        }
        // GET: api/books
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _bookService.GetAllBooksAsync());
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
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
           var createdBook = await _bookService.AddBookAsync(book);
            if (createdBook == null)
            {
                return BadRequest();
            }
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
            var updatedBook = await _bookService.UpdateBookAsync(id, book);
            if (updatedBook == null)
            {
                return NotFound("Knjiga nije pronadjena ili su autor/izdavac nepostojeci.");
            }
            return Ok(updatedBook);
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _bookService.DeleteBookAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
