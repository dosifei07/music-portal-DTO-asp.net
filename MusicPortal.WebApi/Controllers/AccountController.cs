using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.BusinessLogic.Services;
using System.Security.Claims;

namespace MusicPortal.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                var isFirstUser = await _accountService.RegisterAsync(registerDto);
                return Ok(new
                {
                    isFirstUser,
                    message = isFirstUser
                        ? "Вы — первый пользователь портала, аккаунт создан с ролью администратора и одобрен."
                        : "Заявка отправлена администратору портала."
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { field = ex.Property, error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginRequest request)
        {
            UserDTO userDto;
            try
            {
                userDto = await _accountService.ValidateLoginAsync(request.Email, request.Password);
            }
            catch (ValidationException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Name, userDto.Username ?? string.Empty),
                new Claim(ClaimTypes.Email, userDto.Email ?? string.Empty)
            };
            claims.AddRange(userDto.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name ?? string.Empty)));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = request.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(userDto);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                username = User.Identity?.Name,
                email = User.FindFirstValue(ClaimTypes.Email),
                roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value)
            });
        }
    }

    public record LoginRequest(string Email, string Password, bool RememberMe = false);
}