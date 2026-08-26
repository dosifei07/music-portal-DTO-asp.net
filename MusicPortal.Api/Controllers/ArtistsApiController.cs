using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;

namespace music_portal_asp.net.Controllers.Api
{
    [ApiController]
    [Route("api/Artists")]
    [Authorize(Roles = "Admin")]
    public class ArtistsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ArtistsApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await _context.Artists
                .Select(a => new { a.Id, a.Name })
                .ToListAsync();
            return Ok(artists);
        }
    }
}