using MusicPortal.DataAccess.Models;

namespace MusicPortal.DataAccess.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<PagedResult<Comment>> GetBySongIdAsync(int songId, int page = 1, int pageSize = 10);
        Task AddAsync(int songId, int userId, string text);
    }
}
