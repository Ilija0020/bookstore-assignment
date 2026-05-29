using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using BookstoreApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardsController : ControllerBase
    {
        private readonly AwardService _awardService;

        public AwardsController(AwardService awardService)
        {
            _awardService = awardService;
        }

        // GET: api/awards
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _awardService.GetAllAwardsAsync());
        }

        // GET api/awards/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOneAsync(int id)
        {
            var award = await _awardService.GetByAwardIdAsync(id);
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
            Award createdAward = await _awardService.AddAwardAsync(award);
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

            var updatedAward = await _awardService.UpdateAwardAsync(id, award);
            if (updatedAward == null)
            {
                return NotFound();
            }
            return Ok(updatedAward);
        }

        // DELETE api/awards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var success = await _awardService.DeleteAwardAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
