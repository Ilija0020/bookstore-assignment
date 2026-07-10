namespace BookstoreApplication.Services
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task SaveAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
