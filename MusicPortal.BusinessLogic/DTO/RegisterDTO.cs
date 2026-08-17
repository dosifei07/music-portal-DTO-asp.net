using System.ComponentModel.DataAnnotations;

namespace MusicPortal.BusinessLogic.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Поле должно быть установлено.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле должно быть установлено.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Поле должно быть установлено.")]
        public string Password { get; set; } = string.Empty;

        public bool IsArtistRequested { get; set; }
        public string? ArtistName { get; set; }
        public string? Bio { get; set; }
    }
}
