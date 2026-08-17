using System.ComponentModel.DataAnnotations;

namespace MusicPortal.BusinessLogic.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено.")]
        public string? Text { get; set; }

        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public int SongId { get; set; }
    }
}