namespace MusicPortal.BusinessLogic.DTO
{
    public class ArtistDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public int SongCount { get; set; }
        public UserDTO? User { get; set; }
        public List<SongDTO> Songs { get; set; } = new();
    }
}