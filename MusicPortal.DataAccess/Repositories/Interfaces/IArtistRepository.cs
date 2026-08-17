using System.Threading.Tasks;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.DataAccess.Repositories.Interfaces
{
    public interface IArtistRepository : IRepository<Artist>
    {
        Task<Artist?> GetByUserIdAsync(int userId);
    }
}
