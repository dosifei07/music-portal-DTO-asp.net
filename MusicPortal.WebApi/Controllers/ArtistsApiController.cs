using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Services;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public ArtistsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetAll()
        {
            var artists = await _artistService.GetAllAsync();
            return Ok(artists);
        }

        [HttpGet("brief")]
        public async Task<ActionResult<IEnumerable<ArtistDTOBrief>>> GetAllBrief()
        {
            var artists = await _artistService.GetAllBriefAsync();
            return Ok(artists);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ArtistDTO>> GetById(int id)
        {
            var artist = await _artistService.GetByIdAsync(id);
            if (artist == null) return NotFound();
            return Ok(artist);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateArtistRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest(new { field = nameof(request.Name), error = "Имя обязательно." });

            var artist = new Artist
            {
                Name = request.Name,
                Bio = request.Bio ?? string.Empty
            };

            await _artistService.CreateAsync(artist);
            return CreatedAtAction(nameof(GetById), new { id = artist.Id }, artist.Id);
        }
    }

    public record CreateArtistRequest(string Name, string? Bio);
}