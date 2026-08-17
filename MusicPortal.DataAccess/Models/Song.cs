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
        public Artist Artist { get; set; } = null!;
        public ISet<Genre> Genres { get; set; } = new HashSet<Genre>();

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
