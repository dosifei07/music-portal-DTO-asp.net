using Castle.Core.Resource;
using music_portal_asp.net.Resources;
using System.ComponentModel.DataAnnotations;
using Resource = music_portal_asp.net.Resources.Resource;

namespace MusicPortal.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "UsernameRequired")]
        [Display(Name = "UsernameLabel", ResourceType = typeof(Resource))]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "EmailInvalid")]
        [Display(Name = "EmailLabel", ResourceType = typeof(Resource))]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordLength")]
        [Display(Name = "PasswordLabel", ResourceType = typeof(Resource))]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "PasswordsMismatch")]
        [Display(Name = "ConfirmPasswordLabel", ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "RegisterAsArtist", ResourceType = typeof(Resource))]
        public bool IsArtistRequested { get; set; } = false;

        [Display(Name = "ArtistNameLabel", ResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "ArtistNameLength")]
        public string? ArtistName { get; set; }

        [Display(Name = "BioFieldLabel", ResourceType = typeof(Resource))]
        [StringLength(1000, ErrorMessageResourceType = typeof(Resource), ErrorMessageResourceName = "BioLength")]
        public string? Bio { get; set; }
    }
}