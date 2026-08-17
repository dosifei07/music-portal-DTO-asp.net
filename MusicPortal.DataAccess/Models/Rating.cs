namespace MusicPortal.DataAccess.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public Song Song { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
