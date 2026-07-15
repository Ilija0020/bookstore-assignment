using AutoMapper;
using BookstoreApplication.Domain.Common;
using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Repositories;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Services.Interfaces;

namespace BookstoreApplication.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepo _publisherRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PublisherService(IPublisherRepo publisherRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _publisherRepo = publisherRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            var createdPublisher = await _publisherRepo.AddPublisherAsync(publisher);
            await _unitOfWork.SaveAsync();
            return createdPublisher;
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

            await _publisherRepo.UpdatePublisherAsync(existingPublisher);
            await _unitOfWork.SaveAsync();
            return existingPublisher;
        }

        public async Task<bool> DeletePublisherAsync(int id)
        {
            var deleted = await _publisherRepo.DeletePublisherAsync(id);

            if (!deleted)
            {
                return false;
            }
            await _unitOfWork.SaveAsync();
            return true;
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
