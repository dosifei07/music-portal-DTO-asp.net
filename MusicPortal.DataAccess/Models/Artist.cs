namespace MusicPortal.DataAccess.Models
{
    public class Artist
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Bio { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
