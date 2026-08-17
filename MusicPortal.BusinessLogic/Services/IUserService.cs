using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.BusinessLogic.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetPendingUsersAsync();
        Task<UserDTO?> GetByIdAsync(int id);
        Task ApproveAsync(int userId);
        Task RejectAsync(int userId);
        Task DeleteAsync(int userId);

        Task<PagedResult<UserDTO>> GetAllUsersAsync(int page, int pageSize);
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task UpdateUserAsync(int userId, List<int> roleIds, bool isApproved);
    }
}