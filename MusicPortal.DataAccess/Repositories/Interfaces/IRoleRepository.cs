using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.DataAccess.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<List<Role>> GetByIdsAsync(IEnumerable<int> ids);
    }
}
