using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public IssueService(IComicVineConnection comicVineConnection, IIssueRepo issueRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _comicVineConnection = comicVineConnection;
            _issueRepo = issueRepo;
            _mapper = mapper;
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
            var issue = _mapper.Map<Issue>(issueDto);

            issue.CreatedAt = DateTime.UtcNow;

            var result = await _issueRepo.AddIssueAsync(issue);
            await _unitOfWork.SaveAsync();
            return result;
        }
    }
}
