using BookstoreApplication.Domain.Entities;

namespace BookstoreApplication.Domain.Repositories
{
    public interface IIssueRepo
    {
        Task<Issue> AddIssueAsync(Issue issue);
    }
}
