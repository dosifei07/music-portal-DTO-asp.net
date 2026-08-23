using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;

namespace music_portal_asp.net.Controllers.Api
{
    [ApiController]
    [Route("api/Genres")]
    [Authorize(Roles = "Admin")]
    public class GenresApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GenresApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null) return NotFound();
            return genre;
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return Ok(genre);
        }

        [HttpPut]
        public async Task<ActionResult<Genre>> PutGenre(Genre genre)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!_context.Genres.Any(g => g.Id == genre.Id)) return NotFound();

            _context.Update(genre);
            await _context.SaveChangesAsync();

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Genre>> DeleteGenre(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null) return NotFound();

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return Ok(genre);
        }
    }
}