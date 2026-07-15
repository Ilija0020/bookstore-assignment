
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using BookstoreApplication.Services.External;
using System.Net;
using System.Text.Json;

namespace BookstoreApplication.Infrastructure.External.ComicVine
{
    public class ComicVineConnection : IComicVineConnection
    {
        private readonly HttpClient _client;
        private readonly ILogger<ComicVineConnection> _logger;
        private readonly IConfiguration _configuration;

        public ComicVineConnection(HttpClient client, ILogger<ComicVineConnection> logger, IConfiguration configuration)
        {
            _client = client;
            _logger = logger;
            _configuration = configuration;
        }
        private async Task<string> Get(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.UserAgent.ParseAdd("BookstoreApplication");

            HttpResponseMessage response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            using JsonDocument jsonDocument = JsonDocument.Parse(json);

            int statusCode = jsonDocument.RootElement.GetProperty("status_code").GetInt32();

            if (!response.IsSuccessStatusCode)
                HandleUnsuccessfulRequest(response, jsonDocument, statusCode);

            if(statusCode != 1)
                HandleUnsuccessfulRequest(response, jsonDocument, statusCode);

            return jsonDocument.RootElement.GetProperty("results").GetRawText();
        }

        public async Task<List<VolumeDTO>> SearchVolumesByName(string query)
        {
            var url = $"https://comicvine.gamespot.com/api/volumes" +
                $"?api_key={_configuration["ComicVine:ApiKey"]}" +
                $"&format=json" +
                $"&filter=name:{Uri.EscapeDataString(query)}";

            var json = await Get(url);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<VolumeDTO>>(json, options) ?? new List<VolumeDTO>();
        }

        public async Task<List<IssueDTO>> SearchIssuesByVolumeId(int volumeId)
        {
            var url = $"https://comicvine.gamespot.com/api/issues" +
                $"?api_key={_configuration["ComicVine:ApiKey"]}" +
                $"&format=json" +
                $"&filter=volume:{volumeId}";

            var json = await Get(url);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<IssueDTO>>(json, options) ?? new List<IssueDTO>();
        }

        private void HandleUnsuccessfulRequest(HttpResponseMessage response, JsonDocument jsonDocument, int statusCode)
        {
            var errorMessage = "";
            try
            {
                errorMessage = jsonDocument.RootElement.GetProperty("error").GetString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured with message: {ex.Message}");
            }

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new RateLimitException();
            }
            else if (statusCode == 100 || response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedApiAccessException();
            }
            else
            {
                string apiError = string.IsNullOrEmpty(errorMessage) ?
                    "Error occured when sending request to the external API." : errorMessage;
                throw new ApiCommunicationException(apiError);
            }
        }
    }
}
