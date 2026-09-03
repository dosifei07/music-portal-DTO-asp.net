using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.BusinessLogic.Services
{
    public interface ISongService
    {
        Task<PagedResult<SongDTO>> GetFilteredSongsAsync(int? genreId, int? artistId, string? sortBy, bool descending, int page, int pageSize);
        Task<SongDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(SongDTO songDto);
        Task UpdateAsync(SongDTO songDto);
        Task<string?> DeleteAsync(int id);
        Task IncrementPlayCountAsync(int id);
        Task<(string FilePath, string Title)?> GetFileInfoAsync(int id);
    }
}