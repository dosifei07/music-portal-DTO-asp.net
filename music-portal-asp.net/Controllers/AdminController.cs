using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.BusinessLogic.Services;

namespace music_portal_asp.net.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Users(int page = 1)
        {
            var result = await _userService.GetAllUsersAsync(page, 20);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> PendingUsers()
        {
            var pending = await _userService.GetPendingUsersAsync();
            return View(pending);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.AllRoles = await _userService.GetAllRolesAsync();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, List<int> roleIds, bool isApproved = false)
        {
            try
            {
                await _userService.UpdateUserAsync(id, roleIds, isApproved);
                TempData["Message"] = "Пользователь успешно обновлен.";
                return RedirectToAction(nameof(Users));
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
                ViewBag.AllRoles = await _userService.GetAllRolesAsync();
                var user = await _userService.GetByIdAsync(id);
                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                await _userService.RejectAsync(id);
                TempData["Message"] = "Заявка отклонена.";
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                await _userService.ApproveAsync(id);
                TempData["Message"] = "Заявка принята.";
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                TempData["Message"] = "Пользователь удалён.";
            }
            catch (ValidationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Users));
        }
    }
}