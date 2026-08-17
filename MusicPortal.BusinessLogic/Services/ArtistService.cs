using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ArtistService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArtistDTO>> GetAllAsync()
        {
            var artists = await _uow.Artists.GetAllAsync();
            return _mapper.Map<IEnumerable<ArtistDTO>>(artists);
        }

        public async Task<ArtistDTO?> GetByIdAsync(int id)
        {
            var artist = await _uow.Artists.GetByIdAsync(id);
            return _mapper.Map<ArtistDTO?>(artist);
        }

        public async Task CreateAsync(Artist artist)
        {
            await _uow.Artists.AddAsync(artist);
            await _uow.SaveChangesAsync();
        }
    }
}