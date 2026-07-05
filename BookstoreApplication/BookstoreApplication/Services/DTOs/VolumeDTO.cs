using System.Text.Json.Serialization;

namespace BookstoreApplication.Services.DTOs
{
    public class VolumeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("count_of_issues")]
        public int? CountOfIssues { get; set; }
        [JsonPropertyName("start_year")]
        public string? StartYear { get; set; }
    }
}
