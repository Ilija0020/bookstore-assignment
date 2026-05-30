namespace BookstoreApplication.Models
{
    public interface IAwardRepo
    {
        Task<Award> AddAwardAsync(Award award);
        Task<bool> DeleteAwardAsync(int id);
        Task<List<Award>> GetAllAwardsAsync();
        Task<Award?> GetByAwardIdAsync(int id);
        Task<Award> UpdateAwardAsync(Award award);
    }
}