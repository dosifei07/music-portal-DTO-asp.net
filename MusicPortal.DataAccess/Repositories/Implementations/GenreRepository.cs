using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.DataAccess.Repositories.Implementations
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(AppDbContext context) : base(context)
        {
        }
    }
}
