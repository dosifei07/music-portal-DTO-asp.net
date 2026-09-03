using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MusicPortal.Mvc.TagHelpers
{
    /// <summary>
    /// Usage (inside a &lt;th&gt; or anywhere):
    /// <sort-link sort-key="rating" current-sort="@ViewBag.SortBy" current-desc="@ViewBag.Desc"
    ///            action="Index" controller="Songs"
    ///            route-values="@(new Dictionary&lt;string,string&gt; { ["genreId"] = ViewBag.SelectedGenreId?.ToString() })">
    ///     По рейтингу
    /// </sort-link>
    /// Clicking toggles ascending/descending when the same key is clicked again.
    /// </summary>
    [HtmlTargetElement("sort-link")]
    public class SortLinkTagHelper : TagHelper
    {
        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public string SortKey { get; set; } = string.Empty;
        public string? CurrentSort { get; set; }
        public bool CurrentDesc { get; set; } = true;
        public string Action { get; set; } = "Index";
        public string Controller { get; set; } = string.Empty;
        public IDictionary<string, string?>? RouteValues { get; set; }

        private readonly IUrlHelperFactory _urlHelperFactory;

        public SortLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var isActive = string.Equals(CurrentSort, SortKey, StringComparison.OrdinalIgnoreCase);

            // Clicking the already-active column flips direction; clicking a new column defaults to descending.
            var nextDesc = isActive ? !CurrentDesc : true;

            var values = new Dictionary<string, string?>(RouteValues ?? new Dictionary<string, string?>())
            {
                ["sortBy"] = SortKey,
                ["desc"] = nextDesc.ToString().ToLowerInvariant()
            };

            var url = urlHelper.Action(Action, Controller, (object)values);

            output.TagName = "a";
            output.Attributes.SetAttribute("href", url);
            output.Attributes.SetAttribute("class", "text-decoration-none fw-bold");

            if (isActive)
            {
                var arrow = CurrentDesc ? " ▼" : " ▲";
                output.PostContent.AppendHtml(arrow);
            }
        }
    }
}