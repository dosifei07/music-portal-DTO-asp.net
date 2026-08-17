using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.DataAccess.Repositories.Implementations
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rating?> GetByUserAndSongAsync(int userId, int songId)
        {
            return await _context.Ratings.FirstOrDefaultAsync(r => r.User.Id == userId && r.Song.Id == songId);
        }

        public async Task AddOrUpdateAsync(int userId, int songId, int value)
        {
            var existing = await _context.Ratings.FirstOrDefaultAsync(r => r.User.Id == userId && r.Song.Id == songId);
            if (existing != null)
            {
                existing.Value = value;
                _context.Ratings.Update(existing);
                return;
            }

            var song = await _context.Songs.FindAsync(songId);
            var user = await _context.Users.FindAsync(userId);
            if (song == null || user == null) return;

            await _context.Ratings.AddAsync(new Rating { Song = song, User = user, Value = value });
        }
    }
}
