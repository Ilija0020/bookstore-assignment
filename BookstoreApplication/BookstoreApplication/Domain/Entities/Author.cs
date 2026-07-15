namespace BookstoreApplication.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public required string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<AuthorAward> AuthorAwards { get; set; } = new List<AuthorAward>();
    }
}
