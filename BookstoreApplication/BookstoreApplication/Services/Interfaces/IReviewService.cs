using BookstoreApplication.Services.DTOs;

namespace BookstoreApplication.Services.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(NewReviewDTO reviewDto, string userId);
    }
}
