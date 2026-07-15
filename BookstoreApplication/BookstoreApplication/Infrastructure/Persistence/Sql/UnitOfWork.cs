using BookstoreApplication.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookstoreApplication.Infrastructure.Persistence.Sql
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        { 
            if (_transaction is null)
            {
                throw new InvalidOperationException("No active transaction to commit.");
            }

            await SaveAsync();

            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            if (_transaction is null)
            {
                throw new InvalidOperationException("No active transaction to rollback.");
            }

            await _transaction.RollbackAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
