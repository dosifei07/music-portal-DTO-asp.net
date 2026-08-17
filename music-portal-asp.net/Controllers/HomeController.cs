using Microsoft.AspNetCore.Mvc;

namespace music_portal_asp.net.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => RedirectToAction("Index", "Songs");

        [HttpGet]
        public IActionResult Error() => View();
    }
}
