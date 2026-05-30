using BookstoreApplication.Models;
using BookstoreApplication.Repositories;

namespace BookstoreApplication.Services
{
    public class AwardService : IAwardService
    {
        private readonly IAwardRepo _awardRepo;

        public AwardService(IAwardRepo awardRepo)
        {
            _awardRepo = awardRepo;
        }

        public async Task<List<Award>> GetAllAwardsAsync()
        {
            return await _awardRepo.GetAllAwardsAsync();
        }

        public async Task<Award?> GetByAwardIdAsync(int id)
        {
            return await _awardRepo.GetByAwardIdAsync(id);
        }

        public async Task<Award> AddAwardAsync(Award award)
        {
            return await _awardRepo.AddAwardAsync(award);
        }

        public async Task<Award?> UpdateAwardAsync(int id, Award award)
        {
            if (id != award.Id)
            {
                return null;
            }
            var existingAward = await _awardRepo.GetByAwardIdAsync(id);
            if (existingAward == null)
            {
                return null;
            }

            existingAward.Name = award.Name;
            existingAward.Description = award.Description;
            existingAward.StartYear = award.StartYear;

            return await _awardRepo.UpdateAwardAsync(existingAward);
        }

        public async Task<bool> DeleteAwardAsync(int id)
        {
            return await _awardRepo.DeleteAwardAsync(id);
        }
    }
}
