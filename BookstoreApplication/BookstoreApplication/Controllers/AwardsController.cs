using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardsController : ControllerBase
    {
        private readonly AwardRepo _awardRepo;

        public AwardsController(AwardRepo awardRepo)
        {
            _awardRepo = awardRepo;
        }

        // GET: api/awards
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_awardRepo.GetAllAwards());
        }

        // GET api/awards/5
        [HttpGet("{id}")]
        public IActionResult GetOne(int id)
        {
            var award = _awardRepo.GetByAwardId(id);
            if (award == null)
            {
                return NotFound();
            }
            return Ok(award);
        }

        // POST api/awards
        [HttpPost]
        public IActionResult Post(Award award)
        {
            Award createdAward = _awardRepo.AddAward(award);
            return Ok(createdAward);
        }

        // PUT api/awards/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Award award)
        {
            if (id != award.Id)
            {
                return BadRequest();
            }

            var existingAward = _awardRepo.GetByAwardId(id);
            if (existingAward == null)
            {
                return NotFound();
            }

            existingAward.Name = award.Name;
            existingAward.Description = award.Description;
            existingAward.StartYear = award.StartYear;

            _awardRepo.UpdateAward(existingAward);
            return Ok(existingAward);
        }

        // DELETE api/awards/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var award = _awardRepo.DeleteAward(id);
            if (!award)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
