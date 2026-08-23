using Castle.Core.Resource;
using Microsoft.AspNetCore.Http;
using music_portal_asp.net.Resources;
using System.ComponentModel.DataAnnotations;
using Resource = music_portal_asp.net.Resources.Resource;

namespace MusicPortal.Models.ViewModels
{
    public class SongUploadViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TitleRequired")]
        [StringLength(150, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "TitleLength")]
        [Display(Name = "TitleLabel", ResourceType = typeof(Resource))]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "FileRequired")]
        [Display(Name = "FileLabel", ResourceType = typeof(Resource))]
        public IFormFile File { get; set; } = null!;

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ArtistRequired")]
        [Display(Name = "ArtistFieldLabel", ResourceType = typeof(Resource))]
        public int ArtistId { get; set; }

        [Display(Name = "GenresLabel", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "GenresRequired")]
        [MinLength(1, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "GenresRequired")]
        public List<int> GenreIds { get; set; } = new();
    }
}