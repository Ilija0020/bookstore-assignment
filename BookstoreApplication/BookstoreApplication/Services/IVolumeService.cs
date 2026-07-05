using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services
{
    public interface IVolumeService
    {
        Task<List<VolumeDTO>> SearchVolumesByName(string query);
    }
}
