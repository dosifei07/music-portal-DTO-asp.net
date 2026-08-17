using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models.ViewModels
{
    public class SongEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название песни")]
        [StringLength(150, ErrorMessage = "Название не должно превышать 150 символов")]
        [Display(Name = "Название")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите исполнителя")]
        [Display(Name = "Исполнитель")]
        public int ArtistId { get; set; }

        [Display(Name = "Жанры")]
        [Required(ErrorMessage = "Выберите хотя бы один жанр")]
        [MinLength(1, ErrorMessage = "Выберите хотя бы один жанр")]
        public List<int> GenreIds { get; set; } = new();
    }
}
