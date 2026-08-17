using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BusinessLogic.Services;
using MusicPortal.DataAccess.Models;

namespace music_portal_asp.net.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var artists = await _artistService.GetAllAsync();
            return View(artists);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var artist = await _artistService.GetByIdAsync(id);
            if (artist == null) return NotFound();
            return View(artist);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Artist artist)
        {
            if (!ModelState.IsValid)
            {
                return View(artist);
            }

            await _artistService.CreateAsync(artist);
            return RedirectToAction(nameof(Index));
        }
    }
}