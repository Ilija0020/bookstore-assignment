using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Repositories;
using BookstoreApplication.Infrastructure.Persistence.Mongo.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookstoreApplication.Infrastructure.Persistence.Mongo.Repositories
{
    public class ComicNoSqlRepository : IIssueRepo
    {
        private readonly IMongoCollection<ComicIssueDocument> _issuesCollection;

        public ComicNoSqlRepository(IMongoDatabase database, IOptions<MongoDbSettings> options)
        {
            _issuesCollection = database.GetCollection<ComicIssueDocument>(
                options.Value.IssuesCollectionName);
        }

        public async Task<Issue> AddIssueAsync(Issue issue)
        {
            var document = MapToDocument(issue);

            await _issuesCollection.InsertOneAsync(document);

            return issue;
        }

        private static ComicIssueDocument MapToDocument(Issue issue)
        {
            return new ComicIssueDocument
            {
                Name = issue.Name,
                ReleaseDate = issue.ReleaseDate,
                IssueNumber = issue.IssueNumber,
                ImagePath = issue.ImagePath,
                Description = issue.Description,
                ExternalIssueId = issue.ExternalIssueId,
                ExternalVolumeId = issue.ExternalVolumeId,
                PageCount = issue.PageCount,
                Price = issue.Price,
                AvailableCopies = issue.AvailableCopies,
                CreatedAt = issue.CreatedAt
            };
        }
    }
}
