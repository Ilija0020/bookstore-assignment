using System.ComponentModel.DataAnnotations;

namespace BookstoreApplication.Domain.Entities
{
    public class Award
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1900, 2100, ErrorMessage = "Start year must be between 1900 and 2100.")]
        public int StartYear { get; set; }
        public ICollection<AuthorAward> AuthorAwards { get; set; } = new List<AuthorAward>();
    }
}
