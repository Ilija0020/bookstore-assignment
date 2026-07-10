namespace BookstoreApplication.Services.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string ISBN { get; set; }
        public required int PageCount { get; set; }
        public double AverageRating {  get; set; }
        public required string AuthorFullName { get; set; }
        public required string PublisherName { get; set; }
        public int Age { get; set; }
    }
}
