using BookstoreApplication.Data;
using BookstoreApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Repositories
{
    public class AwardRepo : IAwardRepo
    {
        private AppDbContext _context;

        public AwardRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Award>> GetAllAwardsAsync()
        {
            return await _context.Awards.ToListAsync();
        }

        public async Task<Award?> GetByAwardIdAsync(int id)
        {
            return await _context.Awards.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Award> AddAwardAsync(Award award)
        {
            _context.Awards.Add(award);
            await _context.SaveChangesAsync();
            return award;
        }

        public async Task<Award> UpdateAwardAsync(Award award)
        {
            _context.Awards.Update(award);
            await _context.SaveChangesAsync();
            return award;
        }

        public async Task<bool> DeleteAwardAsync(int id)
        {
            Award? award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return false;
            }
            _context.Awards.Remove(award);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
