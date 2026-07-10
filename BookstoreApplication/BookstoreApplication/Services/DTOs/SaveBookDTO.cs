namespace BookstoreApplication.Services.DTOs
{
    public class SaveBookDTO
    {
        public required string Title {  get; set; }
        public int PageCount {  get; set; }
        public DateTime PublishedDate {  get; set; }
        public required string ISBN { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
    }
}
