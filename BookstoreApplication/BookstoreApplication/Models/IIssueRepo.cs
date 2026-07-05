namespace BookstoreApplication.Models
{
    public interface IIssueRepo
    {
        Task<Issue> AddIssueAsync(Issue issue);
    }
}
