using System.Text.Json.Serialization;

namespace BookstoreApplication.Services.DTOs
{
    public class ComicVineImageDTO
    {
        [JsonPropertyName("icon_url")]
        public string? IconUrl { get; set; }

        [JsonPropertyName("tiny_url")]
        public string? TinyUrl { get; set; }

        [JsonPropertyName("thumb_url")]
        public string? ThumbUrl { get; set; }

        [JsonPropertyName("small_url")]
        public string? SmallUrl { get; set; }

        [JsonPropertyName("medium_url")]
        public string? MediumUrl { get; set; }

        [JsonPropertyName("screen_url")]
        public string? ScreenUrl { get; set; }

        [JsonPropertyName("screen_large_url")]
        public string? ScreenLargeUrl { get; set; }

        [JsonPropertyName("super_url")]
        public string? SuperUrl { get; set; }

        [JsonPropertyName("original_url")]
        public string? OriginalUrl { get; set; }
    }
}
