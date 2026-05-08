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
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _awardRepo.GetAllAwardsAsync());
        }

        // GET api/awards/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var award = await _awardRepo.GetByAwardIdAsync(id);
            if (award == null)
            {
                return NotFound();
            }
            return Ok(award);
        }

        // POST api/awards
        [HttpPost]
        public async Task<IActionResult> PostAsync(Award award)
        {
            Award createdAward = await _awardRepo.AddAwardAsync(award);
            return Ok(createdAward);
        }

        // PUT api/awards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Award award)
        {
            if (id != award.Id)
            {
                return BadRequest();
            }

            var existingAward = await _awardRepo.GetByAwardIdAsync(id);
            if (existingAward == null)
            {
                return NotFound();
            }

            existingAward.Name = award.Name;
            existingAward.Description = award.Description;
            existingAward.StartYear = award.StartYear;

            await _awardRepo.UpdateAwardAsync(existingAward);
            return Ok(existingAward);
        }

        // DELETE api/awards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _awardRepo.DeleteAwardAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
