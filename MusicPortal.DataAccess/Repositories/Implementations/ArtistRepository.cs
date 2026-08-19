using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.DataAccess.Repositories.Implementations
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Artist?> GetByIdAsync(int id)
        {
            var artist = await _context.Artists.FirstOrDefaultAsync(a => a.Id == id);
            if (artist != null)
            {
                foreach (var song in artist.Songs)
                {
                    if (song.Ratings.Any())
                        song.Rating = Math.Round(song.Ratings.Average(r => r.Value), 1);
                }
            }
            return artist;
        }

        public override async Task<IEnumerable<Artist>> GetAllAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task<Artist?> GetByUserIdAsync(int userId)
        {
            return await _context.Artists.FirstOrDefaultAsync(a => a.User != null && a.User.Id == userId);
        }
    }
}