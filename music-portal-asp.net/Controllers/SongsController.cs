using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.BusinessLogic.Services;
using MusicPortal.Models.ViewModels;
using System.Security.Claims;

namespace music_portal_asp.net.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongService _songService;
        private readonly IGenreService _genreService;
        private readonly IArtistService _artistService;
        private readonly ICommentService _commentService;
        private readonly IRatingService _ratingService;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] AllowedExtensions = { ".mp3", ".wav", ".flac", ".ogg" };
        private const long MaxFileSizeBytes = 50 * 1024 * 1024;

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

        private int? CurrentUserId => int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

        private async Task<List<ArtistDTO>> PopulateFormLookupsAsync(IEnumerable<int>? selectedGenreIds = null, int? selectedArtistId = null)
        {
            var genres = await _genreService.GetAllAsync();
            ViewBag.Genres = new MultiSelectList(genres, "Id", "Name", selectedGenreIds);

            var artists = (await _artistService.GetAllAsync()).ToList();
            ViewBag.Artists = new SelectList(artists, "Id", "Name", selectedArtistId);
            return artists;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? genreId, string? sortBy, bool desc = true, int page = 1)
        {
            var result = await _songService.GetFilteredSongsAsync(genreId, sortBy, desc, page, 12);

            ViewBag.Genres = await _genreService.GetAllAsync();
            ViewBag.SelectedGenreId = genreId;
            ViewBag.SortBy = sortBy;
            ViewBag.Desc = desc;

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, int commentPage = 1)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();

            ViewBag.Comments = await _commentService.GetBySongIdAsync(id, commentPage, 10);

            if (CurrentUserId is int uid)
            {
                var myRating = await _ratingService.GetByUserAndSongAsync(uid, id);
                ViewBag.MyRating = myRating?.Value;
            }

            return View(song);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateFormLookupsAsync();
            return View(new SongUploadViewModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SongUploadViewModel model)
        {
            var extension = model.File != null ? Path.GetExtension(model.File.FileName).ToLowerInvariant() : "";
            if (model.File == null || model.File.Length == 0)
                ModelState.AddModelError(nameof(model.File), "Файл обязателен.");
            else if (!AllowedExtensions.Contains(extension))
                ModelState.AddModelError(nameof(model.File), "Допустимые форматы: mp3, wav, flac, ogg.");
            else if (model.File.Length > MaxFileSizeBytes)
                ModelState.AddModelError(nameof(model.File), "Файл слишком большой (максимум 50 МБ).");

            var artists = await PopulateFormLookupsAsync(model.GenreIds, model.ArtistId);
            if (!artists.Any(a => a.Id == model.ArtistId))
                ModelState.AddModelError(nameof(model.ArtistId), "Выбранный исполнитель не найден.");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "songs");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await model.File!.CopyToAsync(stream);
            }

            var songDto = new SongDTO
            {
                Title = model.Title,
                FilePath = $"/uploads/songs/{uniqueFileName}",
                ArtistId = model.ArtistId,
                GenreIds = model.GenreIds
            };

            int songId;
            try
            {
                songId = await _songService.CreateAsync(songDto);
            }
            catch (ValidationException ex)
            {
                if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                ModelState.AddModelError(ex.Property, ex.Message);
                return View(model);
            }

            TempData["Message"] = "Песня загружена.";
            return RedirectToAction(nameof(Details), new { id = songId });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var song = await _songService.GetByIdAsync(id);
            if (song == null) return NotFound();

            var vm = new SongEditViewModel
            {
                Id = song.Id,
                Title = song.Title ?? string.Empty,
                ArtistId = song.ArtistId,
                GenreIds = song.GenreIds
            };

            await PopulateFormLookupsAsync(vm.GenreIds, vm.ArtistId);
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SongEditViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateFormLookupsAsync(model.GenreIds, model.ArtistId);
                return View(model);
            }

            var songDto = new SongDTO
            {
                Id = model.Id,
                Title = model.Title,
                ArtistId = model.ArtistId,
                GenreIds = model.GenreIds
            };

            try
            {
                await _songService.UpdateAsync(songDto);
                TempData["Message"] = "Песня обновлена.";
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                await PopulateFormLookupsAsync(model.GenreIds, model.ArtistId);
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var filePath = await _songService.DeleteAsync(id);
                if (filePath != null)
                {
                    var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                }
                TempData["Message"] = "Песня удалена.";
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Play(int id)
        {
            var info = await _songService.GetFileInfoAsync(id);
            if (info == null) return NotFound();

            var fullPath = Path.Combine(_env.WebRootPath, info.Value.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(fullPath)) return NotFound();

            await _songService.IncrementPlayCountAsync(id);

            var contentType = GetContentType(fullPath);
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return File(stream, contentType, enableRangeProcessing: true);
        }

        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            if (User.Identity?.IsAuthenticated != true) return RedirectToAction("Register", "Account");

            var info = await _songService.GetFileInfoAsync(id);
            if (info == null) return NotFound();

            var fullPath = Path.Combine(_env.WebRootPath, info.Value.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(fullPath)) return NotFound();

            var contentType = GetContentType(fullPath);
            var fileName = $"{info.Value.Title}{Path.GetExtension(fullPath)}";
            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return File(stream, contentType, fileName, enableRangeProcessing: true);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int songId, string text)
        {
            if (CurrentUserId is int userId && !string.IsNullOrWhiteSpace(text))
            {
                await _commentService.AddAsync(songId, userId, text.Trim());
            }
            return RedirectToAction(nameof(Details), new { id = songId });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int songId, int value)
        {
            if (CurrentUserId is int userId && value >= 1 && value <= 5)
            {
                await _ratingService.RateAsync(songId, userId, value);
            }
            return RedirectToAction(nameof(Details), new { id = songId });
        }

        private static string GetContentType(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext switch
            {
                ".mp3" => "audio/mpeg",
                ".wav" => "audio/wav",
                ".flac" => "audio/flac",
                ".ogg" => "audio/ogg",
                _ => "application/octet-stream"
            };
        }
    }
}
