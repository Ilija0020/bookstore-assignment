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
        public IActionResult GetAll()
        {
            return Ok(_authorRepo.GetAllAuthors());
        }

        // GET api/authors/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var author = _authorRepo.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        // POST api/authors
        [HttpPost]
        public IActionResult Post(Author author)
        {
            Author createdAuthor = _authorRepo.AddAuthor(author);
            return Ok(createdAuthor);
        }

        // PUT api/authors/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            var existingAuthor = _authorRepo.GetAuthorById(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            existingAuthor.FullName = author.FullName;
            existingAuthor.Biography = author.Biography;
            existingAuthor.DateOfBirth = author.DateOfBirth;

            _authorRepo.UpdateAuthor(existingAuthor);
            return Ok(existingAuthor);
        }

        // DELETE api/authors/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var author = _authorRepo.DeleteAuthor(id);
            if (!author)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
