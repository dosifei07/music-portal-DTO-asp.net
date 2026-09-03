using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.BusinessLogic.Services;
using MusicPortal.DataAccess.Models;
using System.Security.Claims;

namespace MusicPortal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongsController : ControllerBase
    {
        private static readonly string[] AllowedExtensions = { ".mp3", ".wav", ".flac", ".ogg" };
        private const long MaxFileSizeBytes = 50 * 1024 * 1024;

        private readonly ISongService _songService;
        private readonly IGenreService _genreService;
        private readonly IArtistService _artistService;
        private readonly ICommentService _commentService;
        private readonly IRatingService _ratingService;
        private readonly IWebHostEnvironment _env;

        public SongsController(
            ISongService songService, IGenreService genreService, IArtistService artistService,
            ICommentService commentService, IRatingService ratingService, IWebHostEnvironment env)
        {
            _songService = songService;
            _genreService = genreService;
            _artistService = artistService;
            _commentService = commentService;
            _ratingService = ratingService;
            _env = env;
        }

        private int? CurrentUserId =>
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        [HttpGet]
        public async Task<ActionResult<PagedResult<SongDTO>>> GetAll(
            [FromQuery] int? genreId, [FromQuery] int? artistId, [FromQuery] string? sortBy,
            [FromQuery] bool desc = true, [FromQuery] int page = 1)
        {
            var result = await _songService.GetFilteredSongsAsync(genreId, artistId, sortBy, desc, page, 12);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SongDTO>> GetById(int id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();
            return Ok(song);
        }

        [HttpGet("{id:int}/comments")]
        public async Task<ActionResult<PagedResult<CommentDTO>>> GetComments(int id, [FromQuery] int page = 1)
        {
            var comments = await _commentService.GetBySongIdAsync(id, page, 10);
            return Ok(comments);
        }

        [Authorize]
        [HttpPost("{id:int}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] AddCommentRequest request)
        {
            if (CurrentUserId is not int userId || string.IsNullOrWhiteSpace(request.Text))
                return BadRequest();

            await _commentService.AddAsync(id, userId, request.Text.Trim());
            return NoContent();
        }

        [Authorize]
        [HttpPost("{id:int}/rate")]
        public async Task<IActionResult> Rate(int id, [FromBody] RateRequest request)
        {
            if (CurrentUserId is not int userId || request.Value is < 1 or > 5)
                return BadRequest();

            await _ratingService.RateAsync(id, userId, request.Value);
            return NoContent();
        }

        [HttpGet("{id:int}/play")]
        public async Task<IActionResult> Play(int id)
        {
            var info = await _songService.GetFileInfoAsync(id);
            if (info == null) return NotFound();

            var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot",
                info.Value.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(fullPath)) return NotFound();

            await _songService.IncrementPlayCountAsync(id);
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return File(stream, GetContentType(fullPath), enableRangeProcessing: true);
        }

        [Authorize]
        [HttpGet("{id:int}/download")]
        public async Task<IActionResult> Download(int id)
        {
            var info = await _songService.GetFileInfoAsync(id);
            if (info == null) return NotFound();

            var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot",
                info.Value.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(fullPath)) return NotFound();

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return File(stream, GetContentType(fullPath), $"{info.Value.Title}{Path.GetExtension(fullPath)}");
        }

        [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(MaxFileSizeBytes)]
        public async Task<ActionResult<int>> Create([FromForm] SongUploadRequest request)
        {
            var extension = request.File != null ? Path.GetExtension(request.File.FileName).ToLowerInvariant() : "";

            if (request.File == null || request.File.Length == 0)
                ModelState.AddModelError(nameof(request.File), "Файл обязателен.");
            else if (!AllowedExtensions.Contains(extension))
                ModelState.AddModelError(nameof(request.File), "Допустимые форматы: mp3, wav, flac, ogg.");
            else if (request.File.Length > MaxFileSizeBytes)
                ModelState.AddModelError(nameof(request.File), "Файл слишком большой (максимум 50 МБ).");

            var artists = await _artistService.GetAllBriefAsync();
            if (!artists.Any(a => a.Id == request.ArtistId))
                ModelState.AddModelError(nameof(request.ArtistId), "Выбранный исполнитель не найден.");

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var storageFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "uploads", "songs");
            Directory.CreateDirectory(storageFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(storageFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await request.File!.CopyToAsync(stream);
            }

            var songDto = new SongDTO
            {
                Title = request.Title,
                FilePath = $"/uploads/songs/{uniqueFileName}",
                ArtistId = request.ArtistId,
                Genres = (request.GenreIds ?? new List<int>()).Select(id => new GenreDTO { Id = id }).ToList()
            };

            int songId;
            try
            {
                songId = await _songService.CreateAsync(songDto);
            }
            catch (ValidationException ex)
            {
                if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                return BadRequest(new { field = ex.Property, error = ex.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = songId }, songId);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SongEditRequest request)
        {
            if (id != request.Id) return BadRequest();

            var songDto = new SongDTO
            {
                Id = request.Id,
                Title = request.Title,
                ArtistId = request.ArtistId,
                Genres = request.GenreIds.Select(gid => new GenreDTO { Id = gid }).ToList()
            };

            try
            {
                await _songService.UpdateAsync(songDto);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { field = ex.Property, error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var filePath = await _songService.DeleteAsync(id);
                if (filePath != null)
                {
                    var fullPath = Path.Combine(_env.ContentRootPath, "wwwroot",
                        filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        private static string GetContentType(string path) => Path.GetExtension(path).ToLowerInvariant() switch
        {
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".flac" => "audio/flac",
            ".ogg" => "audio/ogg",
            _ => "application/octet-stream"
        };
    }

    public record AddCommentRequest(string Text);
    public record RateRequest(int Value);

    public class SongUploadRequest
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
        public int ArtistId { get; set; }
        public List<int>? GenreIds { get; set; }
    }

    public record SongEditRequest(int Id, string Title, int ArtistId, List<int> GenreIds);
}