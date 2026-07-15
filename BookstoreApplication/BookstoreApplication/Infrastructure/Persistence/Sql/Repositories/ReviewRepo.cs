using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Infrastructure.Persistence.Sql.Repositories
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly AppDbContext _context;

        public ReviewRepo(AppDbContext context)
        {
            _context = context;
        }
        public Task<Review> AddReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            return Task.FromResult(review);
        }

        public async Task<double> GetAverageRatingForBookAsync(int bookId)
        {
            return await _context.Reviews
                .Where(review=>review.BookId == bookId)
                .AverageAsync(review => review.Rating);
        }
    }
}
