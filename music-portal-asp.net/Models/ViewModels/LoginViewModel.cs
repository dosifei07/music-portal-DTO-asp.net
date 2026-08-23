using Castle.Core.Resource;
using music_portal_asp.net.Resources;
using System.ComponentModel.DataAnnotations;
using Resource = music_portal_asp.net.Resources.Resource;

namespace MusicPortal.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailInvalid")]
        [Display(Name = "EmailLabel", ResourceType = typeof(Resource))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "PasswordLabel", ResourceType = typeof(Resource))]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "RememberMe", ResourceType = typeof(Resource))]
        public bool RememberMe { get; set; } = false;
    }
}