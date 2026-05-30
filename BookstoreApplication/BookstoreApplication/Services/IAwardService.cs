using BookstoreApplication.Models;

namespace BookstoreApplication.Services
{
    public interface IAwardService
    {
        Task<Award> AddAwardAsync(Award award);
        Task<bool> DeleteAwardAsync(int id);
        Task<List<Award>> GetAllAwardsAsync();
        Task<Award?> GetByAwardIdAsync(int id);
        Task<Award?> UpdateAwardAsync(int id, Award award);
    }
}