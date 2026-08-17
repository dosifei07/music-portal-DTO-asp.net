namespace MusicPortal.DataAccess.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRoleRepository Roles { get; }
        IArtistRepository Artists { get; }
        ISongRepository Songs { get; }
        IGenreRepository Genres { get; }
        ICommentRepository Comments { get; }
        IRatingRepository Ratings { get; }

        Task<bool> SaveChangesAsync();
    }
}
    