using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.DataAccess.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IUserRepository? _users;
        private IRoleRepository? _roles;
        private IArtistRepository? _artists;
        private ISongRepository? _songs;
        private IGenreRepository? _genres;
        private ICommentRepository? _comments;
        private IRatingRepository? _ratings;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IRoleRepository Roles => _roles ??= new RoleRepository(_context);
        public IArtistRepository Artists => _artists ??= new ArtistRepository(_context);
        public ISongRepository Songs => _songs ??= new SongRepository(_context);
        public IGenreRepository Genres => _genres ??= new GenreRepository(_context);
        public ICommentRepository Comments => _comments ??= new CommentRepository(_context);
        public IRatingRepository Ratings => _ratings ??= new RatingRepository(_context);

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
