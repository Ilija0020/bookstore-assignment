namespace BookstoreApplication.Models
{
    public interface IPublisherRepo
    {
        Task<Publisher> AddPublisherAsync(Publisher publisher);
        Task<bool> DeletePublisherAsync(int id);
        Task<List<Publisher>> GetAllPublishersAsync();
        Task<Publisher?> GetPublisherByIdAsync(int id);
        Task<Publisher> UpdatePublisherAsync(Publisher publisher);
    }
}