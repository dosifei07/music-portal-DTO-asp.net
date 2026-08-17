using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicPortal.BusinessLogic.Profiles;
using MusicPortal.BusinessLogic.Services;
using MusicPortal.DataAccess.Data;
using MusicPortal.DataAccess.Repositories.Implementations;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMusicPortalContext(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(MappingProfile).Assembly);
            });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IRatingService, RatingService>();

            return services;
        }
    }
}
