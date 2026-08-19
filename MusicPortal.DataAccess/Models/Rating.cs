namespace MusicPortal.DataAccess.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public virtual Song Song { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}