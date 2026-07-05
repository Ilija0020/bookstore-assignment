using System.Text.Json.Serialization;

namespace BookstoreApplication.Services.DTOs
{
    public class IssueDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [JsonPropertyName("issue_number")]
        public string? IssueNumber { get; set; }
        [JsonPropertyName("cover_date")]
        public DateOnly? CoverDate { get; set; }
        [JsonPropertyName("store_date")]
        public DateOnly? StoreDate { get; set; }
        public string? Description { get; set; }
        public ComicVineImageDTO? Image { get; set; }
    }
}
