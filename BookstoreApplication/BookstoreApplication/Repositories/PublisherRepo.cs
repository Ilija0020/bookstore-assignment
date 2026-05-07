using BookstoreApplication.Data;
using BookstoreApplication.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BookstoreApplication.Repositories
{
    public class PublisherRepo
    {

        private AppDbContext _context;

        public PublisherRepo(AppDbContext context)
        {
            _context = context;
        }

        public List<Publisher> GetAllPublishers()
        {
            return _context.Publishers.ToList();
        }

        public Publisher? GetPublisherById(int id)
        {
            return _context.Publishers.FirstOrDefault(p => p.Id == id);
        }

        public Publisher AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
            _context.SaveChanges();
            return publisher;
        }

        public Publisher UpdatePublisher(Publisher publisher)
        {
            _context.Publishers.Update(publisher);
            _context.SaveChanges();
            return publisher;
        }

        public bool DeletePublisher(int id)
        {
            Publisher? publisher = _context.Publishers.Find(id);
            if (publisher == null)
            {
                return false;
            }
            _context.Publishers.Remove(publisher);
            _context.SaveChanges();
            return true;
        }
    }
}
