using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.DataAccess.Repositories.Implementations
{
    public class SongRepository : Repository<Song>, ISongRepository
    {
        public SongRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Song?> GetByIdAsync(int id)
        {
            return await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Genres)
                .Include(s => s.Comments).ThenInclude(c => c.User)
                .Include(s => s.Ratings)
                .AsSplitQuery() 
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<IEnumerable<Song>> GetAllAsync()
        {
            return await _context.Songs
                .Include(s => s.Artist)
                .Include(s => s.Genres)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<PagedResult<Song>> GetFilteredSongsAsync(int? genreId, int? artistId, string? sortBy, bool descending = true, int page = 1, int pageSize = 12)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 12 : pageSize;

            IQueryable<Song> baseQuery = _context.Songs.AsQueryable();
            if (genreId.HasValue)
            {
                baseQuery = baseQuery.Where(s => s.Genres.Any(g => g.Id == genreId.Value));
            }
            if (artistId.HasValue)
            {
                baseQuery = baseQuery.Where(s => s.ArtistId == artistId.Value);
            }

            var totalCount = await baseQuery.CountAsync();

            IQueryable<Song> ordered = sortBy?.ToLower() switch
            {
                "plays" => descending ? baseQuery.OrderByDescending(s => s.PlayCount) : baseQuery.OrderBy(s => s.PlayCount),
                "date" => descending ? baseQuery.OrderByDescending(s => s.UploadDate) : baseQuery.OrderBy(s => s.UploadDate),
                "rating" => descending
                    ? baseQuery.OrderByDescending(s => s.Ratings.Any() ? s.Ratings.Average(r => r.Value) : 0)
                    : baseQuery.OrderBy(s => s.Ratings.Any() ? s.Ratings.Average(r => r.Value) : 0),
                _ => baseQuery.OrderByDescending(s => s.UploadDate)
            };

            var items = await ordered
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(s => s.Artist)
                .Include(s => s.Ratings)
                .Include(s => s.Genres)
                .AsSplitQuery()
                .ToListAsync();


            foreach (var song in items)
            {
                if (song.Ratings.Any())
                {
                    song.Rating = Math.Round(song.Ratings.Average(r => r.Value), 1);
                }
            }

            return new PagedResult<Song> { Items = items, TotalCount = totalCount, Page = page, PageSize = pageSize };
        }

        public async Task IncrementPlayCountAsync(int songId)
        {
            await _context.Songs
                .Where(s => s.Id == songId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.PlayCount, x => x.PlayCount + 1));
        }

        public async Task<(string FilePath, string Title)?> GetFileInfoAsync(int id)
        {
            var result = await _context.Songs
                .Where(s => s.Id == id)
                .Select(s => new { s.FilePath, s.Title })
                .FirstOrDefaultAsync();

            return result == null ? null : (result.FilePath, result.Title);
        }
        public async Task UpdateSongRatingAsync(int songId)
        {
            var song = await _context.Songs
                .Include(s => s.Ratings)
                .FirstOrDefaultAsync(s => s.Id == songId);

            if (song != null)
            {
                song.Rating = song.Ratings.Any()
                    ? Math.Round(song.Ratings.Average(r => r.Value), 1)
                    : 0.0;

                await _context.SaveChangesAsync();
            }
        }
        public async Task RecalculateAllRatingsAsync()
        {
            var songs = await _context.Songs.Include(s => s.Ratings).ToListAsync();
            foreach (var song in songs)
            {
                song.Rating = song.Ratings.Any()
                    ? Math.Round(song.Ratings.Average(r => r.Value), 1)
                    : 0.0;
            }
            await _context.SaveChangesAsync();
        }
    }
}
