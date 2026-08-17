namespace MusicPortal.BusinessLogic.DTO
{
    public class RatingDTO
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public int UserId { get; set; }
        public int Value { get; set; }
    }
}