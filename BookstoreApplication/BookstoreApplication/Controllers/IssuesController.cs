using BookstoreApplication.Services.Interfaces;
using BookstoreApplication.Services.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }
        [Authorize(Roles = "Editor")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchIssuesByVolumeId([FromQuery] int volumeId)
        {
            var issues = await _issueService.SearchIssuesByVolumeId(volumeId);
            return Ok(issues);
        }

        [Authorize(Roles = "Editor")]
        [HttpPost]
        public async Task<IActionResult> AddIssue([FromBody] SaveIssueDTO issueDto)
        {
            var issue = await _issueService.AddIssueAsync(issueDto);
            return Ok(issue);
        }
    }
}
