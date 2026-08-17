namespace MusicPortal.BusinessLogic.DTO
{
    public class SongDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public string? ArtistName { get; set; }
        public double Rating { get; set; }
        public int PlayCount { get; set; }
        public DateTime UploadDate { get; set; }
        public List<int> GenreIds { get; set; } = new();
        public List<string> GenreNames { get; set; } = new();
    }
}