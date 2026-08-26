using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.Api.Controllers
{
    public class ArtistRefDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class GenreRefDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class SongApiModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? FilePath { get; set; }
        public ArtistRefDTO Artist { get; set; } = new();
        public List<GenreRefDTO> Genres { get; set; } = new();
    }

    [ApiController]
    [Route("api/Songs")]
    [Authorize(Roles = "Admin")]
    public class SongsApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        private static readonly string[] AllowedExtensions = { ".mp3", ".wav", ".flac", ".ogg" };

        public SongsApiController(AppDbContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _context = context;
            _env = env;
            _config = config;
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
                    Artist = new { s.Artist.Id, s.Artist.Name },
                    Genres = s.Genres.Select(g => new { g.Id, g.Name })
                })
                .SingleOrDefaultAsync();
            if (song == null) return NotFound();
            return Ok(song);
        }

        [HttpPost("upload")]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return BadRequest("Допустимые форматы: mp3, wav, flac, ogg.");

            var configuredPath = _config["Storage:SongsUploadPath"] ?? "wwwroot/uploads/songs";
            var uploadsFolder = Path.GetFullPath(Path.Combine(_env.ContentRootPath, configuredPath));
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { filePath = $"/uploads/songs/{uniqueFileName}" });
        }

        [HttpPost]
        public async Task<IActionResult> PostSong(SongApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Artist == null || model.Artist.Id <= 0)
                return BadRequest("Не указан исполнитель.");
            if (string.IsNullOrWhiteSpace(model.FilePath))
                return BadRequest("Не загружен аудиофайл.");

            var artist = await _context.Artists.FindAsync(model.Artist.Id);
            if (artist == null) return BadRequest("Исполнитель не найден.");

            var genreIds = model.Genres.Select(g => g.Id).ToList();
            var genres = genreIds.Any()
                ? await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync()
                : new List<Genre>();

            var song = new Song
            {
                Title = model.Title ?? string.Empty,
                FilePath = model.FilePath,
                ArtistId = artist.Id,
                UploadDate = DateTime.UtcNow,
                PlayCount = 0,
                Genres = genres.ToHashSet()
            };

            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                song.Id,
                song.Title,
                song.FilePath,
                Artist = new { artist.Id, artist.Name },
                Genres = song.Genres.Select(g => new { g.Id, g.Name })
            });
        }

        [HttpPut]
        public async Task<IActionResult> PutSong(SongApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Artist == null || model.Artist.Id <= 0)
                return BadRequest("Не указан исполнитель.");

            var song = await _context.Songs.Include(s => s.Genres).SingleOrDefaultAsync(s => s.Id == model.Id);
            if (song == null) return NotFound();

            var artist = await _context.Artists.FindAsync(model.Artist.Id);
            if (artist == null) return BadRequest("Исполнитель не найден.");

            song.Title = model.Title ?? song.Title;
            song.ArtistId = artist.Id;
            if (!string.IsNullOrWhiteSpace(model.FilePath))
                song.FilePath = model.FilePath;

            var genreIds = model.Genres.Select(g => g.Id).ToList();
            song.Genres.Clear();
            if (genreIds.Any())
            {
                var genres = await _context.Genres.Where(g => genreIds.Contains(g.Id)).ToListAsync();
                foreach (var g in genres) song.Genres.Add(g);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                song.Id,
                song.Title,
                song.FilePath,
                Artist = new { artist.Id, artist.Name },
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

            return NoContent();
        }
    }
}