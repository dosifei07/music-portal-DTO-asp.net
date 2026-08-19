using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public RatingService(IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<RatingDTO?> GetByUserAndSongAsync(int userId, int songId)
        {
            var rating = await _ratingRepository.GetByUserAndSongAsync(userId, songId);
            return _mapper.Map<RatingDTO?>(rating);
        }

        public Task RateAsync(RatingDTO ratingDto) => RateAsync(ratingDto.SongId, ratingDto.UserId, ratingDto.Value);

        public async Task RateAsync(int songId, int userId, int value)
        {
            await _ratingRepository.AddOrUpdateAsync(userId, songId, value);
        }
    }
}