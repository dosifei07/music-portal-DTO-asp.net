using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.BusinessLogic.Services;
using MusicPortal.Models.ViewModels;

namespace music_portal_asp.net.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllAsync();
            return View(genres);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new GenreViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new GenreDTO { Name = model.Name };

            try
            {
                await _genreService.CreateAsync(dto);
                TempData["Message"] = "Жанр добавлен.";
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null) return NotFound();

            var vm = new GenreViewModel { Id = genre.Id, Name = genre.Name };
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GenreViewModel model)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid)
                return View(model);

            var dto = new GenreDTO { Id = model.Id, Name = model.Name };

            try
            {
                await _genreService.UpdateAsync(dto);
                TempData["Message"] = "Жанр обновлён.";
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _genreService.DeleteAsync(id);
                TempData["Message"] = "Жанр удалён.";
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}