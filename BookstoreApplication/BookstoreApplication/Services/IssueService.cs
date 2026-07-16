using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Repositories;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using BookstoreApplication.Services.External;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services
{
    public class IssueService : IIssueService
    {
        private readonly IComicVineConnection _comicVineConnection;
        private readonly IIssueRepo _issueRepo;
        private readonly IUnitOfWork _unitOfWork;

        public IssueService(IComicVineConnection comicVineConnection, IIssueRepo issueRepo, IUnitOfWork unitOfWork)
        {
            _comicVineConnection = comicVineConnection;
            _issueRepo = issueRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<IssueDTO>> SearchIssuesByVolumeId(int volumeId)
        {
            if (volumeId <= 0)
            {
                throw new BadRequestException("Volume ID is required.");
            }

            return await _comicVineConnection.SearchIssuesByVolumeId(volumeId);
        }

        public async Task<Issue> AddIssueAsync(SaveIssueDTO issueDto)
        {
            var details = await _comicVineConnection.GetIssueById(issueDto.ExternalIssueId);

            if (details.Volume is null)
            {
                throw new ApiCommunicationException("Comic Vine issue volume could not be read.");
            }

            var issue = new Issue
            {
                Name = details.Name ?? string.Empty,
                ReleaseDate = details.StoreDate ?? details.CoverDate,
                IssueNumber = details.IssueNumber,

                ImagePath = details.Image?.OriginalUrl ??
                    details.Image?.SuperUrl ??
                    details.Image?.MediumUrl ??
                    details.Image?.SmallUrl,

                Description = string.IsNullOrWhiteSpace(details.Description)
                    ? details.Deck
                    : details.Description,

                ExternalIssueId = details.Id,
                ExternalVolumeId = details.Volume.Id,

                PageCount = issueDto.PageCount,
                Price = issueDto.Price,
                AvailableCopies = issueDto.AvailableCopies,

                CreatedAt = DateTime.UtcNow
            };

            var result = await _issueRepo.AddIssueAsync(issue);
            await _unitOfWork.SaveAsync();
            return result;
        }
    }
}
