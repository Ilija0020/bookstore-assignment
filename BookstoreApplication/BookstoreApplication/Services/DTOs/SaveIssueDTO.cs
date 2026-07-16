using System.ComponentModel.DataAnnotations;

namespace BookstoreApplication.Services.DTOs
{
    public class SaveIssueDTO
    {
        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "External issue ID is required.")]
        public int ExternalIssueId { get; set; }

        [Range(
            1,
            int.MaxValue,
            ErrorMessage = "Page count must be greater than 0.")]
        public int PageCount { get; set; }

        [Range(
            typeof(decimal),
            "0",
            "79228162514264337593543950335",
            ErrorMessage = "Price cannot be negative.")]
        public decimal Price { get; set; }

        [Range(
            0,
            int.MaxValue,
            ErrorMessage = "Available copies cannot be negative.")]
        public int AvailableCopies { get; set; }
    }
}
