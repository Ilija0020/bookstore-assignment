using BookstoreApplication.Domain.Entities;

namespace BookstoreApplication.Domain.Repositories
{
    public interface IReviewRepo
    {
        Task<Review> AddReviewAsync(Review review);
        Task<double> GetAverageRatingForBookAsync(int bookId);
    }
}
