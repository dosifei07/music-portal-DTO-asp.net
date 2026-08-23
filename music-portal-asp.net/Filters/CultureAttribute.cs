using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace music_portal_asp.net.Filters
{
    public class CultureAttribute : Attribute, IActionFilter
    {
        private static readonly List<string> SupportedCultures = new() { "ru", "en", "uk", "de", "fr" };

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var cultureName = context.HttpContext.Request.Cookies["lang"];

            if (string.IsNullOrEmpty(cultureName) || !SupportedCultures.Contains(cultureName))
                cultureName = "ru";

            var culture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}