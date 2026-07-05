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
        public async Task<Issue> AddIssueAsync(Issue issue)
        {
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return issue;
        }

    }
}
