using System.Threading.Tasks;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.DataAccess.Repositories.Interfaces
{
    public interface ISongRepository : IRepository<Song>
    {
        Task<PagedResult<Song>> GetFilteredSongsAsync(int? genreId, string? sortBy, bool descending = true, int page = 1, int pageSize = 12);
        Task IncrementPlayCountAsync(int songId);
        Task<(string FilePath, string Title)?> GetFileInfoAsync(int id);
        Task UpdateSongRatingAsync(int songId);
    }
}
