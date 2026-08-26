using MusicPortal.DataAccess.Models;

namespace MusicPortal.DataAccess.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<PagedResult<Comment>> GetBySongIdAsync(int songId, int page = 1, int pageSize = 10);
        Task<Comment?> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetAllAsync();
        Task AddAsync(int songId, int userId, string text);
        Task<bool> UpdateAsync(int id, string text);
        Task<bool> DeleteAsync(int id);
    }
}