
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

        public ComicVineConnection(HttpClient client, ILogger<ComicVineConnection> logger)
        {
            _client = client;
            _logger = logger;
        }
        public async Task<string> Get(string url)
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
