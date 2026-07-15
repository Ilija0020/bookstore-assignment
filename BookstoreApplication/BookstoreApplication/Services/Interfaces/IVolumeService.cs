using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IVolumeService
    {
        Task<List<VolumeDTO>> SearchVolumesByName(string query);
    }
}
