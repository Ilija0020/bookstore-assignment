using BookstoreApplication.Models;
using BookstoreApplication.Repositories;

namespace BookstoreApplication.Services
{
    public class AwardService : IAwardService
    {
        private readonly IAwardRepo _awardRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AwardService(IAwardRepo awardRepo, IUnitOfWork unitOfWork)
        {
            _awardRepo = awardRepo;
            _unitOfWork = unitOfWork;
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
            var createdAward = await _awardRepo.AddAwardAsync(award);
            await _unitOfWork.SaveAsync();
            return createdAward;
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

            await _awardRepo.UpdateAwardAsync(existingAward);
            await _unitOfWork.SaveAsync();
            return existingAward;
        }

        public async Task<bool> DeleteAwardAsync(int id)
        {
            var deleted = await _awardRepo.DeleteAwardAsync(id);
            if (!deleted)
            {
                return false;
            }
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
