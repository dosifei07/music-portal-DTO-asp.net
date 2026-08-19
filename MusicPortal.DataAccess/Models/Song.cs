namespace MusicPortal.DataAccess.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
        public int PlayCount { get; set; } = 0;
        public double Rating { get; set; } = 0;

        public int ArtistId { get; set; }
        public virtual Artist Artist { get; set; } = null!;
        public virtual ISet<Genre> Genres { get; set; } = new HashSet<Genre>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}