using BookstoreApplication.Data;
using BookstoreApplication.Models;

namespace BookstoreApplication.Repositories
{
    public class AwardRepo
    {
        private AppDbContext _context;

        public AwardRepo(AppDbContext context)
        {
            _context = context;
        }

        public List<Award> GetAllAwards()
        {
            return _context.Awards.ToList();
        }

        public Award? GetByAwardId(int id)
        {
            return _context.Awards.FirstOrDefault(a => a.Id == id);
        }

        public Award AddAward(Award award)
        {
            _context.Awards.Add(award);
            _context.SaveChanges();
            return award;
        }

        public Award UpdateAward(Award award)
        {
            _context.Awards.Update(award);
            _context.SaveChanges();
            return award;
        }

        public bool DeleteAward(int id)
        {
            Award? award = _context.Awards.Find(id);
            if (award == null)
            {
                return false;
            }
            _context.Awards.Remove(award);
            _context.SaveChanges();
            return true;
        }
    }
}
