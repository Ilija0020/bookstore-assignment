using BookstoreApplication.Data;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using BookstoreApplication.Services;
using BookstoreApplication.Services.DTOs;
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
        // GET api/books/sortTypes
        [HttpGet("sortTypes")]
        public IActionResult GetSortTypes()
        {
            return Ok(_bookService.GetSortTypes());
        }
        // GET api/books/sort?sortType=2
        [HttpGet("sort")]
        public async Task<IActionResult> GetSortedBooks([FromQuery] int sortType)
        {
            return Ok(await _bookService.GetAllSortedAsync(sortType));
        }
        // POST api/books/filterAndSort?sortType=2
        [HttpPost("filterAndSort")]
        public async Task<IActionResult> GetFilteredAndSortedBooks([FromBody] BookFilter filter, [FromQuery] int sortType)
        {
            return Ok(await _bookService.GetAllFilteredAndSortedAsync(filter, sortType));
        }

        // GET api/books/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public async Task<IActionResult> PostAsync(Book book)
        {
           var createdBook = await _bookService.AddBookAsync(book);
           return Ok(createdBook);
        }

        // PUT api/books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest("ID in path does not match ID in body.");
            }
            var updatedBook = await _bookService.UpdateBookAsync(id, book);
            return Ok(updatedBook);
        }

        // DELETE api/books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}
