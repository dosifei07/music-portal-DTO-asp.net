using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;

namespace music_portal_asp.net.Controllers.Api
{
    [ApiController]
    [Route("api/Roles")]
    [Authorize(Roles = "Admin")]
    public class RolesApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles
                .Select(r => new { r.Id, r.Name })
                .ToListAsync();
            return Ok(roles);
        }
    }
}