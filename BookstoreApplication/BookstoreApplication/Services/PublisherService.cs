using BookstoreApplication.Models;
using BookstoreApplication.Repositories;

namespace BookstoreApplication.Services
{
    public class PublisherService
    {
        private readonly PublisherRepo _publisherRepo;

        public PublisherService(PublisherRepo publisherRepo)
        {
            _publisherRepo = publisherRepo;
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
    }
}
