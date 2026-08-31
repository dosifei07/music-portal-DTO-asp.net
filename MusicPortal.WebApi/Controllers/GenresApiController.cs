using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.BusinessLogic.Services;

namespace MusicPortal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreDTO>>> GetAll()
        {
            var genres = await _genreService.GetAllAsync();
            return Ok(genres);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenreDTO>> GetById(int id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null) return NotFound();
            return Ok(genre);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<GenreDTO>> Create([FromBody] GenreDTO dto)
        {
            try
            {
                var id = await _genreService.CreateAsync(dto);
                dto.Id = id;
                return CreatedAtAction(nameof(GetById), new { id }, dto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { field = ex.Property, error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] GenreDTO dto)
        {
            if (id != dto.Id) return BadRequest();

            try
            {
                await _genreService.UpdateAsync(dto);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return NotFound(new { field = ex.Property, error = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _genreService.DeleteAsync(id);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return NotFound(new { field = ex.Property, error = ex.Message });
            }
        }
    }
}