namespace MusicPortal.DataAccess.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Song Song { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
