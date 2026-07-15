using System.ComponentModel.DataAnnotations;

namespace BookstoreApplication.Domain.Entities
{
    public class AuthorAward
    {
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int AwardId { get; set; }
        public Award Award { get; set; }

        [Range(1900, 2100, ErrorMessage = "Year received must be between 1900 and 2100.")]
        public int YearReceived { get; set; }
    }
}
