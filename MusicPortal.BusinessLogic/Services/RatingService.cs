using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RatingService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<RatingDTO?> GetByUserAndSongAsync(int userId, int songId)
        {
            var rating = await _uow.Ratings.GetByUserAndSongAsync(userId, songId);
            return _mapper.Map<RatingDTO?>(rating);
        }

        public Task RateAsync(RatingDTO ratingDto) => RateAsync(ratingDto.SongId, ratingDto.UserId, ratingDto.Value);
        public async Task RateAsync(int songId, int userId, int value)
        {
            await _uow.Ratings.AddOrUpdateAsync(userId, songId, value);
            await _uow.SaveChangesAsync();
        }
    }
}
