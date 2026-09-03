using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace MusicPortal.Mvc.TagHelpers
{
    /// <summary>
    /// Usage:
    /// <pagination current-page="Model.Page" total-pages="Model.TotalPages"
    ///              action="Index" controller="Songs"
    ///              route-values="@(new Dictionary<string,string?> { ["genreId"] = ViewBag.SelectedGenreId?.ToString() })">
    /// </pagination>
    /// </summary>
    [HtmlTargetElement("pagination")]
    public class PaginationTagHelper : TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string Action { get; set; } = "Index";
        public string Controller { get; set; } = string.Empty;

        /// <summary>Extra query-string values to preserve across page links (filters, sort, etc.)</summary>
        public IDictionary<string, string?>? RouteValues { get; set; }

        private readonly IUrlHelperFactory _urlHelperFactory;

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (TotalPages <= 1)
            {
                output.SuppressOutput();
                return;
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            output.TagName = "nav";
            output.Attributes.SetAttribute("class", "mt-4");
            output.Attributes.SetAttribute("aria-label", "Page navigation");

            var sb = new StringBuilder();
            sb.Append("<ul class=\"pagination justify-content-center\">");

            AppendPageLink(sb, urlHelper, CurrentPage - 1, "Назад", disabled: CurrentPage <= 1);

            for (int p = 1; p <= TotalPages; p++)
            {
                AppendPageLink(sb, urlHelper, p, p.ToString(), active: p == CurrentPage);
            }

            AppendPageLink(sb, urlHelper, CurrentPage + 1, "Вперёд", disabled: CurrentPage >= TotalPages);

            sb.Append("</ul>");
            output.Content.SetHtmlContent(sb.ToString());
        }

        private void AppendPageLink(StringBuilder sb, IUrlHelper urlHelper, int page, string label, bool active = false, bool disabled = false)
        {
            var values = new Dictionary<string, string?>(RouteValues ?? new Dictionary<string, string?>())
            {
                ["page"] = page.ToString()
            };

            var url = urlHelper.Action(Action, Controller, (object)values);
            var itemClass = "page-item" + (active ? " active" : "") + (disabled ? " disabled" : "");

            sb.Append($"<li class=\"{itemClass}\">");
            if (disabled)
            {
                sb.Append($"<span class=\"page-link\">{label}</span>");
            }
            else
            {
                sb.Append($"<a class=\"page-link\" href=\"{url}\">{label}</a>");
            }
            sb.Append("</li>");
        }
    }
}