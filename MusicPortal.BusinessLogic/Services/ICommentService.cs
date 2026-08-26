using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.BusinessLogic.Services
{
    public interface ICommentService
    {
        Task<PagedResult<CommentDTO>> GetBySongIdAsync(int songId, int page = 1, int pageSize = 10);
        Task<IEnumerable<CommentDTO>> GetAllAsync();
        Task<CommentDTO?> GetByIdAsync(int id);
        Task AddAsync(CommentDTO commentDto);
        Task AddAsync(int songId, int userId, string text);
        Task UpdateAsync(int id, string text);
        Task DeleteAsync(int id);
    }
}