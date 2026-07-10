using System.ComponentModel.DataAnnotations;

namespace BookstoreApplication.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        [Range(1,5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
