using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using BookstoreApplication.Services.External;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services
{
    public class VolumeService : IVolumeService
    {
        private readonly IComicVineConnection _comicVineConnection;

        public VolumeService(IComicVineConnection comicVineConnection)
        {
            _comicVineConnection = comicVineConnection;
        }
        public async Task<List<VolumeDTO>> SearchVolumesByName(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new BadRequestException("Search query is required.");
            }

            return await _comicVineConnection.SearchVolumesByName(query);
        }
    }
}
