using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;

namespace MusicPortal.BusinessLogic.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Song, SongDTO>()
                .ForMember(d => d.ArtistName, o => o.MapFrom(s => s.Artist != null ? s.Artist.Name : null))
                .ForMember(d => d.GenreIds, o => o.MapFrom(s => s.Genres.Select(g => g.Id)))
                .ForMember(d => d.GenreNames, o => o.MapFrom(s => s.Genres.Select(g => g.Name)));

            CreateMap<Genre, GenreDTO>();

            CreateMap<Artist, ArtistDTO>()
                .ForMember(d => d.SongCount, o => o.MapFrom(s => s.Songs.Count));

            CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
            CreateMap<CommentDTO, Comment>();

            CreateMap<Rating, RatingDTO>();
            CreateMap<RatingDTO, Rating>();

            CreateMap<User, UserDTO>()
                .ForMember(d => d.RoleNames, o => o.MapFrom(s => s.Roles.Select(r => r.Name)));

            CreateMap<Role, RoleDTO>();
        }
    }
}
