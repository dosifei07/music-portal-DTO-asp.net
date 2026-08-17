using System.Threading.Tasks;
using MusicPortal.BusinessLogic.DTO;

namespace MusicPortal.BusinessLogic.Services
{
    public interface IRatingService
    {
        Task<RatingDTO?> GetByUserAndSongAsync(int userId, int songId);
        Task RateAsync(RatingDTO ratingDto);
        Task RateAsync(int songId, int userId, int value);
    }
}