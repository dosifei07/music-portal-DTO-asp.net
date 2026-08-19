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
                .ForMember(d => d.Genres, o => o.MapFrom(s => s.Genres));

            CreateMap<Genre, GenreDTO>();
            CreateMap<Artist, ArtistDTO>()
                .ForMember(d => d.SongCount, o => o.MapFrom(s => s.Songs.Count))
                .ForMember(d => d.User, o => o.MapFrom(s => s.User));
            CreateMap<Artist, ArtistDTOBrief>(); ;

            CreateMap<Comment, CommentDTO>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.User != null ? s.User.Username : null));
            CreateMap<CommentDTO, Comment>();

            CreateMap<Rating, RatingDTO>();
            CreateMap<RatingDTO, Rating>();

            CreateMap<User, UserDTO>()
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.Roles));

            CreateMap<Role, RoleDTO>();
        }
    }
}
