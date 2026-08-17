using System.Collections.Generic;
using System.Threading.Tasks;
using MusicPortal.BusinessLogic.DTO;

namespace MusicPortal.BusinessLogic.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDTO>> GetAllAsync();
        Task<GenreDTO?> GetByIdAsync(int id);
        Task<int> CreateAsync(GenreDTO dto);
        Task UpdateAsync(GenreDTO dto);
        Task DeleteAsync(int id);
    }
}