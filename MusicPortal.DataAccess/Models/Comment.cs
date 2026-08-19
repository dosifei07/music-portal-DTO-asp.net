namespace MusicPortal.DataAccess.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual Song Song { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}