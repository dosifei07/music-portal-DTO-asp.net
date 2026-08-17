using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.BusinessLogic.Services
{
    public interface ICommentService
    {
        Task<PagedResult<CommentDTO>> GetBySongIdAsync(int songId, int page = 1, int pageSize = 10);
        Task AddAsync(CommentDTO commentDto);
        Task AddAsync(int songId, int userId, string text);
    }
}
