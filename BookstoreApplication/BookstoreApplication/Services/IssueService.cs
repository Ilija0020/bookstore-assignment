using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Exceptions;
using BookstoreApplication.Services.External;
using System.Text.Json;

namespace BookstoreApplication.Services
{
    public class IssueService : IIssueService
    {
        private readonly IComicVineConnection _comicVineConnection;
        private readonly IConfiguration _configuration;
        private readonly IIssueRepo _issueRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public IssueService(IComicVineConnection comicVineConnection, IConfiguration configuration, IIssueRepo issueRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _comicVineConnection = comicVineConnection;
            _configuration = configuration;
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

            var url = $"https://comicvine.gamespot.com/api/issues" +
                $"?api_key={_configuration["ComicVine:ApiKey"]}" +
                $"&format=json" +
                $"&filter=volume:{volumeId}";

            var json = await _comicVineConnection.Get(url);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<IssueDTO>>(json, options)!;
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
