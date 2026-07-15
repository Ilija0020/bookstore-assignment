using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IIssueService
    {
        Task<List<IssueDTO>> SearchIssuesByVolumeId(int volumeId);
        Task<Issue> AddIssueAsync(SaveIssueDTO issueDto);
    }
}
