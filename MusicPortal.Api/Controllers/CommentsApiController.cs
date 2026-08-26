using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.Api.Controllers
{
    public class CommentCreateDTO
    {
        public int SongId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    public class CommentUpdateDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/Comments")]
    [Authorize(Roles = "Admin")]
    public class CommentsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _context.Comments
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new
                {
                    c.Id,
                    c.Text,
                    c.CreatedAt,
                    Song = new { c.Song.Id, c.Song.Title },
                    User = new { c.User.Id, c.User.Username }
                })
                .ToListAsync();
            return Ok(comments);
        }

        [HttpGet("bySong/{songId}")]
        public async Task<IActionResult> GetBySong(int songId, int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Comments.Where(c => c.Song.Id == songId).OrderByDescending(c => c.CreatedAt);
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new
                {
                    c.Id,
                    c.Text,
                    c.CreatedAt,
                    Song = new { c.Song.Id, c.Song.Title },
                    User = new { c.User.Id, c.User.Username }
                })
                .ToListAsync();

            return Ok(new { items, totalCount, page, pageSize });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _context.Comments
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Text,
                    c.CreatedAt,
                    Song = new { c.Song.Id, c.Song.Title },
                    User = new { c.User.Id, c.User.Username }
                })
                .SingleOrDefaultAsync();

            if (comment == null) return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(CommentCreateDTO dto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Text))
                return BadRequest("Текст комментария обязателен.");

            var song = await _context.Songs.FindAsync(dto.SongId);
            if (song == null) return BadRequest("Песня не найдена.");

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return BadRequest("Пользователь не найден.");

            var comment = new Comment
            {
                Song = song,
                User = user,
                Text = dto.Text,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                comment.Id,
                comment.Text,
                comment.CreatedAt,
                Song = new { song.Id, song.Title },
                User = new { user.Id, user.Username }
            });
        }

        [HttpPut]
        public async Task<IActionResult> PutComment(CommentUpdateDTO dto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Text))
                return BadRequest("Текст комментария обязателен.");

            var comment = await _context.Comments
                .Include(c => c.Song)
                .Include(c => c.User)
                .SingleOrDefaultAsync(c => c.Id == dto.Id);
            if (comment == null) return NotFound();

            comment.Text = dto.Text;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                comment.Id,
                comment.Text,
                comment.CreatedAt,
                Song = new { comment.Song.Id, comment.Song.Title },
                User = new { comment.User.Id, comment.User.Username }
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id == id);
            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}