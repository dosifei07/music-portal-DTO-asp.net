using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;

namespace music_portal_asp.net.Controllers.Api
{
    public class UserApiModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool IsApproved { get; set; }
        public List<int> RoleIds { get; set; } = new();
    }

    [ApiController]
    [Route("api/Users")]
    [Authorize(Roles = "Admin")]
    public class UsersApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.IsApproved,
                    u.CreatedAt,
                    Roles = u.Roles.Select(r => new { r.Id, r.Name })
                })
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.IsApproved,
                    u.CreatedAt,
                    Roles = u.Roles.Select(r => new { r.Id, r.Name })
                })
                .SingleOrDefaultAsync();
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (string.IsNullOrWhiteSpace(model.Password)) return BadRequest("Пароль обязателен.");
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                return BadRequest("Пользователь с таким Email уже существует.");

            var roles = model.RoleIds.Any()
                ? await _context.Roles.Where(r => model.RoleIds.Contains(r.Id)).ToListAsync()
                : new List<Role>();

            var user = new User
            {
                Username = model.Username ?? string.Empty,
                Email = model.Email ?? string.Empty,
                PasswordHash = PasswordHashHelper.HashPassword(model.Password),
                IsApproved = model.IsApproved,
                CreatedAt = DateTime.UtcNow,
                Roles = roles
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.IsApproved,
                user.CreatedAt,
                Roles = user.Roles.Select(r => new { r.Id, r.Name })
            });
        }

        [HttpPut]
        public async Task<ActionResult<User>> PutUser(UserApiModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _context.Users.Include(u => u.Roles).SingleOrDefaultAsync(u => u.Id == model.Id);
            if (user == null) return NotFound();

            user.Username = model.Username ?? user.Username;
            user.Email = model.Email ?? user.Email;
            user.IsApproved = model.IsApproved;

            if (!string.IsNullOrWhiteSpace(model.Password))
                user.PasswordHash = PasswordHashHelper.HashPassword(model.Password);

            user.Roles.Clear();
            if (model.RoleIds.Any())
            {
                var roles = await _context.Roles.Where(r => model.RoleIds.Contains(r.Id)).ToListAsync();
                foreach (var role in roles) user.Roles.Add(role);
            }

            await _context.SaveChangesAsync();
            return Ok(new
            {
                user.Id,
                user.Username,
                user.Email,
                user.IsApproved,
                user.CreatedAt,
                Roles = user.Roles.Select(r => new { r.Id, r.Name })
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}