namespace MusicPortal.DataAccess.Models
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public virtual User? User { get; set; }
        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}