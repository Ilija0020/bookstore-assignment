using BookstoreApplication.Data;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorRepo _authorRepo;
        public AuthorsController(AuthorRepo authorRepo)
        {
            _authorRepo = authorRepo;
        }
        // GET: api/authors
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _authorRepo.GetAllAuthorsAsync());
        }

        // GET api/authors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var author = await _authorRepo.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        // POST api/authors
        [HttpPost]
        public async Task<IActionResult> PostAsync(Author author)
        {
            Author createdAuthor = await _authorRepo.AddAuthorAsync(author);
            return Ok(createdAuthor);
        }

        // PUT api/authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            var existingAuthor =await _authorRepo.GetAuthorByIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            existingAuthor.FullName = author.FullName;
            existingAuthor.Biography = author.Biography;
            existingAuthor.DateOfBirth = author.DateOfBirth;

            await _authorRepo.UpdateAuthorAsync(existingAuthor);
            return Ok(existingAuthor);
        }

        // DELETE api/authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _authorRepo.DeleteAuthorAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
