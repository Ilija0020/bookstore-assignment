using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services
{
    public interface IReviewService
    {
        Task AddReviewAsync(NewReviewDTO reviewDto, string userId);
    }
}
