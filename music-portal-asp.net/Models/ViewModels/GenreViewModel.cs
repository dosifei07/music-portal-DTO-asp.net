using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models.ViewModels
{
    public class GenreViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название жанра")]
        [StringLength(50, ErrorMessage = "Название не должно превышать 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;
    }
}