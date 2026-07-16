using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookstoreApplication.Infrastructure.Persistence.Mongo.Documents
{
    public class ComicIssueDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [BsonDateOnlyOptions(BsonType.DateTime)]
        public DateOnly? ReleaseDate { get; set; }
        public string? IssueNumber { get; set; }

        public string? ImagePath { get; set; }

        public string? Description { get; set; }

        public int ExternalIssueId { get; set; }

        public int ExternalVolumeId { get; set; }

        public int PageCount { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public int AvailableCopies { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; }
    }
}
