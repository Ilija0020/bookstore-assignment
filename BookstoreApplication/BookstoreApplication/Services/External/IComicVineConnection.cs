using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services.External
{
    public interface IComicVineConnection
    {
        Task<List<VolumeDTO>> SearchVolumesByName(string query);
        Task<List<IssueDTO>> SearchIssuesByVolumeId(int volumeId);
    }
}
