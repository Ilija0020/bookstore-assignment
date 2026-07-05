namespace BookstoreApplication.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly? ReleaseDate { get; set; }
        public string? IssueNumber { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }
        public int ExternalIssueId { get; set; }
        public int ExternalVolumeId { get; set; }
        public int PageCount { get; set; }
        public decimal Price { get; set; }
        public int AvailableCopies { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
