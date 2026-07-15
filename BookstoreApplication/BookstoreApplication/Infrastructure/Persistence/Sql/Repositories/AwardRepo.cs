using BookstoreApplication.Domain.Entities;
using BookstoreApplication.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Infrastructure.Persistence.Sql.Repositories
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

        public Task<Award> AddAwardAsync(Award award)
        {
            _context.Awards.Add(award);
            return Task.FromResult(award);
        }

        public Task<Award> UpdateAwardAsync(Award award)
        {
            _context.Awards.Update(award);
            return Task.FromResult(award);
        }

        public async Task<bool> DeleteAwardAsync(int id)
        {
            Award? award = await _context.Awards.FindAsync(id);
            if (award == null)
            {
                return false;
            }
            _context.Awards.Remove(award);
            return true;
        }
    }
}
