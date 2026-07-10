using BookstoreApplication.Data;
using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories
{
    public class IssueRepo : IIssueRepo
    {
        private readonly AppDbContext _context;

        public IssueRepo(AppDbContext context)
        {
            _context = context;
        }
        public Task<Issue> AddIssueAsync(Issue issue)
        {
            _context.Issues.Add(issue);
            return Task.FromResult(issue);
        }

    }
}
