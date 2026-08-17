using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetPendingUsersAsync();
        Task<PagedResult<User>> GetAllUsersAsync(int page = 1, int pageSize = 20);
        Task<bool> AnyUsersExistAsync();
        Task<bool> ApproveUserAsync(User user, IEnumerable<Role> roles);
        Task<bool> SetRolesAsync(User user, IEnumerable<Role> roles);
    }
}