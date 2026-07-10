using System.ComponentModel.DataAnnotations;

namespace BookstoreApplication.Services.DTOs
{
    public class NewReviewDTO
    {
        [Range(1, int.MaxValue)]
        public int BookId { get; set; }
        [Range(1, 5)]
        public int Rating {  get; set; }
        public string? Comment { get; set; }
    }
}
