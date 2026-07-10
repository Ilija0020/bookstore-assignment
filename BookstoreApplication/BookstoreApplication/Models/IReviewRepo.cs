namespace BookstoreApplication.Models
{
    public interface IReviewRepo
    {
        Task<Review> AddReviewAsync(Review review);
        Task<double> GetAverageRatingForBookAsync(int bookId);
    }
}
