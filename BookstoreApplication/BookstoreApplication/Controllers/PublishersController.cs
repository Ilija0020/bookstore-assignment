using BookstoreApplication.Data;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {

        private readonly PublisherRepo _publisherRepo;

        public PublishersController(PublisherRepo publisherRepo)
        {
            _publisherRepo = publisherRepo;
        }
        // GET: api/publishers
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_publisherRepo.GetAllPublishers());
        }

        // GET api/publishers/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var publisher = _publisherRepo.GetPublisherById(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }

        // POST api/publishers
        [HttpPost]
        public IActionResult Post(Publisher publisher)
        {
            Publisher createdPublisher = _publisherRepo.AddPublisher(publisher);
            return Ok(createdPublisher);
        }

        // PUT api/publishers/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return BadRequest();
            }

            var existingPublisher = _publisherRepo.GetPublisherById(id);
            if (existingPublisher == null)
            {
                return NotFound();
            }

            _publisherRepo.UpdatePublisher(publisher);
            return Ok(publisher);
        }

        // DELETE api/publishers/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var publisher = _publisherRepo.DeletePublisher(id);
            if (!publisher)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
