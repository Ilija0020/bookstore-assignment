using BookstoreApplication.Models;
using BookstoreApplication.Services.DTOs;
using BookstoreApplication.Utils;

namespace BookstoreApplication.Services
{
    public interface IPublisherService
    {
        Task<Publisher> AddPublisherAsync(Publisher publisher);
        Task<bool> DeletePublisherAsync(int id);
        Task<List<Publisher>> GetAllPublishersAsync();
        Task<Publisher?> GetPublisherByIdAsync(int id);
        Task<Publisher?> UpdatePublisherAsync(int id, Publisher publisher);
        Task<IEnumerable<PublisherDTO>> GetAllSortedAsync(int sortType);
        public List<SortTypeOption> GetSortTypes();
    }
}