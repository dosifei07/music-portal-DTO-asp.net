using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.Api.Controllers
{
    public class GenreCreateDTO
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GenreUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

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
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _context.Genres.Select(g => new { g.Id, g.Name }).ToListAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenre(int id)
        {
            var genre = await _context.Genres
                .Where(g => g.Id == id)
                .Select(g => new { g.Id, g.Name })
                .SingleOrDefaultAsync();
            if (genre == null) return NotFound();
            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> PostGenre(GenreCreateDTO dto)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Название жанра обязательно.");

            var genre = new Genre { Name = dto.Name };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return Ok(new { genre.Id, genre.Name });
        }

        [HttpPut]
        public async Task<IActionResult> PutGenre(GenreUpdateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var genre = await _context.Genres.FindAsync(dto.Id);
            if (genre == null) return NotFound();

            genre.Name = dto.Name;
            await _context.SaveChangesAsync();

            return Ok(new { genre.Id, genre.Name });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);
            if (genre == null) return NotFound();

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}