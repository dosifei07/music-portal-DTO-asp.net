using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.DataAccess.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Comment>> GetBySongIdAsync(int songId, int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Comments.Where(c => c.Song.Id == songId).OrderByDescending(c => c.CreatedAt);
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.User)
                .ToListAsync();

            return new PagedResult<Comment>(items, totalCount, page, pageSize);
        }
        public async Task AddAsync(int songId, int userId, string text)
        {
            var song = await _context.Songs.FindAsync(songId);
            var user = await _context.Users.FindAsync(userId);
            if (song == null || user == null) return;

            await _context.Comments.AddAsync(new Comment
            {
                Song = song,
                User = user,
                Text = text,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
