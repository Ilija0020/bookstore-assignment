using AutoMapper;
using BookstoreApplication.Models;
using BookstoreApplication.Repositories;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Utils;

namespace BookstoreApplication.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepo _publisherRepo;
        private readonly IMapper _mapper;

        public PublisherService(IPublisherRepo publisherRepo, IMapper mapper)
        {
            _publisherRepo = publisherRepo;
            _mapper = mapper;
        }

        public async Task<List<Publisher>> GetAllPublishersAsync()
        {
            return await _publisherRepo.GetAllPublishersAsync();
        }

        public async Task<Publisher?> GetPublisherByIdAsync(int id)
        {
            return await _publisherRepo.GetPublisherByIdAsync(id);
        }

        public async Task<Publisher> AddPublisherAsync(Publisher publisher)
        {
            return await _publisherRepo.AddPublisherAsync(publisher);
        }

        public async Task<Publisher?> UpdatePublisherAsync(int id, Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return null;
            }
            var existingPublisher = await _publisherRepo.GetPublisherByIdAsync(id);
            if (existingPublisher == null)
            {
                return null;
            }
            existingPublisher.Name = publisher.Name;
            existingPublisher.Address = publisher.Address;
            existingPublisher.Website = publisher.Website;

            return await _publisherRepo.UpdatePublisherAsync(existingPublisher);
        }

        public async Task<bool> DeletePublisherAsync(int id)
        {
            return await _publisherRepo.DeletePublisherAsync(id);
        }

        public async Task<IEnumerable<PublisherDTO>> GetAllSortedAsync(int sortType)
        {
            var publisher = await _publisherRepo.GetAllSortedAsync(sortType);

            var dtos = publisher.Select(_mapper.Map<PublisherDTO>);
            return dtos;
        }
        public List<SortTypeOption> GetSortTypes()
        {
            return _publisherRepo.GetSortTypes();
        }
    }
}
