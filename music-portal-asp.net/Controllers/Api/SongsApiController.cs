using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;

namespace music_portal_asp.net.Controllers.Api
{
    public class SongApiModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? FilePath { get; set; }
        public int ArtistId { get; set; }
        public List<int> GenreIds { get; set; } = new();
    }

    [ApiController]
    [Route("api/Songs")]
    [Authorize(Roles = "Admin")]
    public class SongsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SongsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSongs()
        {
            var songs = await _context.Songs
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.FilePath,
                    s.ArtistId,
                    Artist = new { s.Artist.Id, s.Artist.Name },
                    Genres = s.Genres.Select(g => new { g.Id, g.Name })
                })
                .ToListAsync();
            return Ok(songs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSong(int id)
        {
            var song = await _context.Songs
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    s.Title,
                    s.FilePath,
                    s.ArtistId,
                    Artist = new { s.Artist.Id, s.Artist.Name },
                    Genres = s.Genres.Select(g => new { g.Id, g.Name })
                })
                .SingleOrDefaultAsync();
            if (song == null) return NotFound();
            return Ok(song);
        }

        [HttpPost]
        public async Task<IActionResult> PostSong(SongApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _context.Artists.AnyAsync(a => a.Id == model.ArtistId))
                return BadRequest("Исполнитель не найден.");

            var genres = model.GenreIds.Any()
                ? await _context.Genres.Where(g => model.GenreIds.Contains(g.Id)).ToListAsync()
                : new List<Genre>();

            var song = new Song
            {
                Title = model.Title ?? string.Empty,
                FilePath = model.FilePath ?? string.Empty,
                ArtistId = model.ArtistId,
                UploadDate = DateTime.UtcNow,
                PlayCount = 0,
                Genres = genres.ToHashSet()
            };

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            var artist = await _context.Artists
                .Where(a => a.Id == song.ArtistId)
                .Select(a => new { a.Id, a.Name })
                .SingleOrDefaultAsync();

            return Ok(new
            {
                song.Id,
                song.Title,
                song.FilePath,
                song.ArtistId,
                Artist = artist,
                Genres = song.Genres.Select(g => new { g.Id, g.Name })
            });
        }

        [HttpPut]
        public async Task<IActionResult> PutSong(SongApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var song = await _context.Songs
                .Include(s => s.Genres)
                .SingleOrDefaultAsync(s => s.Id == model.Id);
            if (song == null) return NotFound();

            song.Title = model.Title ?? song.Title;
            song.ArtistId = model.ArtistId;

            song.Genres.Clear();
            if (model.GenreIds.Any())
            {
                var genres = await _context.Genres.Where(g => model.GenreIds.Contains(g.Id)).ToListAsync();
                foreach (var g in genres) song.Genres.Add(g);
            }

            await _context.SaveChangesAsync();

            var artist = await _context.Artists
                .Where(a => a.Id == song.ArtistId)
                .Select(a => new { a.Id, a.Name })
                .SingleOrDefaultAsync();

            return Ok(new
            {
                song.Id,
                song.Title,
                song.FilePath,
                song.ArtistId,
                Artist = artist,
                Genres = song.Genres.Select(g => new { g.Id, g.Name })
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            var song = await _context.Songs.SingleOrDefaultAsync(s => s.Id == id);
            if (song == null) return NotFound();

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            return Ok(new { song.Id, song.Title });
        }
    }
}