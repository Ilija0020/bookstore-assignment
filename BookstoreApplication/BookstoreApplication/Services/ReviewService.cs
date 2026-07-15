using AutoMapper;
using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Repositories;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepo _reviewRepo;
        private readonly IBookRepo _bookRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(IReviewRepo reviewRepo, IBookRepo bookRepo, IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReviewService> logger)
        {
            _reviewRepo = reviewRepo;
            _bookRepo = bookRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddReviewAsync(NewReviewDTO reviewDto, string userId)
        {
            var book = await _bookRepo.GetBookByIdAsync(reviewDto.BookId);
            if (book == null)
            {
                _logger.LogWarning("Failed to create review. Book with ID {BookId} was not found.", reviewDto.BookId);
                throw new NotFoundException(reviewDto.BookId);
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var review = _mapper.Map<Review>(reviewDto);

                review.UserId = userId;
                review.CreatedAt = DateTime.UtcNow;
                review.Comment = string.IsNullOrWhiteSpace(review.Comment)
                    ? null : review.Comment.Trim();

                await _reviewRepo.AddReviewAsync(review);

                await _unitOfWork.SaveAsync();

                var averageRating = await _reviewRepo.GetAverageRatingForBookAsync(reviewDto.BookId);

                book.AverageRating = averageRating;

                await _bookRepo.UpdateBookAsync(book);

                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Review created successfully for book ID {BookId} by user ID {UserId}. New avg rating is {AverageRating}.",
                    reviewDto.BookId,
                    userId,
                    averageRating);
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
