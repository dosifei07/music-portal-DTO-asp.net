using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Имя должно быть от 3 до 100 символов")]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Зарегистрироваться как Исполнитель (требует подтверждения)")]
        public bool IsArtistRequested { get; set; } = false;

        [Display(Name = "Сценическое имя (если выбрали роль Исполнителя)")]
        [StringLength(100, ErrorMessage = "Имя исполнителя не должно превышать 100 символов")]
        public string? ArtistName { get; set; }

        [Display(Name = "О себе / Биография")]
        [StringLength(1000, ErrorMessage = "Описание не должно превышать 1000 символов")]
        public string? Bio { get; set; }
    }
}
