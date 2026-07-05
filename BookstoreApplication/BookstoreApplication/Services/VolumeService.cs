using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using BookstoreApplication.Services.External;
using System.Text.Json;

namespace BookstoreApplication.Services
{
    public class VolumeService : IVolumeService
    {
        private readonly IComicVineConnection _comicVineConnection;
        private readonly IConfiguration _configuration;

        public VolumeService(IComicVineConnection comicVineConnection, IConfiguration configuration)
        {
            _comicVineConnection = comicVineConnection;
            _configuration = configuration;
        }
        public async Task<List<VolumeDTO>> SearchVolumesByName(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new BadRequestException("Search query is required.");
            }
            var url = $"https://comicvine.gamespot.com/api/volumes" +
                $"?api_key={_configuration["ComicVine:ApiKey"]}" +
                $"&format=json" +
                $"&filter=name:{Uri.EscapeDataString(query)}";

            var json = await _comicVineConnection.Get(url);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<VolumeDTO>>(json, options)!;
        }
    }
}
