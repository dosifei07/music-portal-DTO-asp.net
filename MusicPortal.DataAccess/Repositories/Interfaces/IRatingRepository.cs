using MusicPortal.DataAccess.Models;

namespace MusicPortal.DataAccess.Repositories.Interfaces
{
    public interface IRatingRepository
    {
        Task<Rating?> GetByUserAndSongAsync(int userId, int songId);
        Task AddOrUpdateAsync(int userId, int songId, int value);
    }
}
