namespace MusicPortal.DataAccess.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}