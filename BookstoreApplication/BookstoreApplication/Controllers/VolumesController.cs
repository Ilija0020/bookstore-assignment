using BookstoreApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VolumesController : ControllerBase
    {
        private readonly IVolumeService _volumeService;

        public VolumesController(IVolumeService volumeService)
        {
            _volumeService = volumeService;
        }

        [Authorize(Roles = "Editor")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchVolumesByName([FromQuery] string query)
        {
            var volumes = await _volumeService.SearchVolumesByName(query);
            return Ok(volumes);
        }
    }
}
