using BookstoreApplication.Domain.Common;
using BookstoreApplication.Domain.Entities;

namespace BookstoreApplication.Domain.Repositories
{
    public interface IPublisherRepo
    {
        Task<Publisher> AddPublisherAsync(Publisher publisher);
        Task<bool> DeletePublisherAsync(int id);
        Task<List<Publisher>> GetAllPublishersAsync();
        Task<Publisher?> GetPublisherByIdAsync(int id);
        Task<Publisher> UpdatePublisherAsync(Publisher publisher);
        Task<IEnumerable<Publisher>> GetAllSortedAsync(int sortType);
        public List<SortTypeOption> GetSortTypes();
    }
}
