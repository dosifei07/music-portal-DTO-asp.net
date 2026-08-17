using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Некорректный формат Email")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; } = false;
    }
}
