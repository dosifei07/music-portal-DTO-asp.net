using Castle.Core.Resource;
using music_portal_asp.net.Resources;
using System.ComponentModel.DataAnnotations;
using Resource = music_portal_asp.net.Resources.Resource;

namespace MusicPortal.Models.ViewModels
{
    public class GenreViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "GenreNameRequired")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "GenreNameLength")]
        [Display(Name = "NameColumn", ResourceType = typeof(Resource))]
        public string Name { get; set; } = string.Empty;
    }
}