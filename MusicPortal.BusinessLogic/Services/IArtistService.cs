using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.BusinessLogic.Services
{
    public interface IArtistService
    {
        Task<IEnumerable<ArtistDTO>> GetAllAsync();
        Task<ArtistDTO?> GetByIdAsync(int id);
        Task CreateAsync(Artist artist);
    }
}