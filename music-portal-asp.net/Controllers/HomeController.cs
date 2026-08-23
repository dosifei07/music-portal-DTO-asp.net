using Microsoft.AspNetCore.Mvc;

namespace music_portal_asp.net.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index() => RedirectToAction("Index", "Songs");

        [HttpGet]
        public IActionResult Error() => View();

        [HttpGet]
        public IActionResult ChangeCulture(string lang, string? returnUrl)
        {
            var supported = new List<string> { "ru", "en", "uk", "de", "fr" };
            if (!supported.Contains(lang)) lang = "ru";

            Response.Cookies.Append("lang", lang, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Songs");
        }
    }
}
