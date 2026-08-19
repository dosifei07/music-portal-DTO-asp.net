using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtistDTO>> GetAllAsync()
        {
            var artists = await _artistRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArtistDTO>>(artists);
        }

        public async Task<IEnumerable<ArtistDTOBrief>> GetAllBriefAsync()
        {
            var artists = await _artistRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArtistDTOBrief>>(artists);
        }

        public async Task<ArtistDTO?> GetByIdAsync(int id)
        {
            var artist = await _artistRepository.GetByIdAsync(id);
            return _mapper.Map<ArtistDTO?>(artist);
        }

        public async Task CreateAsync(Artist artist)
        {
            await _artistRepository.AddAsync(artist);
        }
    }
}