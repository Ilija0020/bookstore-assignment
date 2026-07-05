using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services
{
    public interface IIssueService
    {
        Task<List<IssueDTO>> SearchIssuesByVolumeId(int volumeId);
        Task<Issue> AddIssueAsync(SaveIssueDTO issueDto);
    }
}
