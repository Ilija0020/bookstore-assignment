using BookstoreApplication.Data;
using BookstoreApplication.Models;
using BookstoreApplication.Utils;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories
{
    public class PublisherRepo : IPublisherRepo
    {

        private AppDbContext _context;

        public PublisherRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Publisher>> GetAllPublishersAsync()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> GetPublisherByIdAsync(int id)
        {
            return await _context.Publishers.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Publisher> AddPublisherAsync(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();
            return publisher;
        }

        public async Task<Publisher> UpdatePublisherAsync(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            await _context.SaveChangesAsync();
            return publisher;
        }

        public async Task<bool> DeletePublisherAsync(int id)
        {
            Publisher? publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return false;
            }
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return true;
        }

        public List<SortTypeOption> GetSortTypes()
        {
            List<SortTypeOption> options = new List<SortTypeOption>();
            var enumValues = Enum.GetValues(typeof(PublisherSortType));
            foreach (PublisherSortType sortType in enumValues)
            {
                options.Add(new SortTypeOption(sortType));
            }
            return options;
        }

        public async Task<IEnumerable<Publisher>> GetAllSortedAsync(int sortType)
        {
            IQueryable<Publisher> publishers = _context.Publishers;
            publishers = SortPublishers(publishers, sortType);
            return await publishers.ToListAsync();
        }

        private static IQueryable<Publisher> SortPublishers(IQueryable<Publisher> publishers, int sortType)
        {
            return sortType switch
            {
                (int)PublisherSortType.NAME_ASCENDING => publishers.OrderBy(p => p.Name),
                (int)PublisherSortType.NAME_DESCENDING => publishers.OrderByDescending(p => p.Name),
                (int)PublisherSortType.ADDRESS_ASCENDING => publishers.OrderBy(p => p.Address),
                (int)PublisherSortType.ADDRESS_DESCENDING => publishers.OrderByDescending(p => p.Address),
                _ => publishers.OrderBy(p => p.Name),
            };
        }
    }
}
