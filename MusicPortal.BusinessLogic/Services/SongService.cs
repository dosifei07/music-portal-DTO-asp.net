using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class SongService : ISongService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public SongService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResult<SongDTO>> GetFilteredSongsAsync(int? genreId, string? sortBy, bool descending, int page, int pageSize)
        {
            var pagedResult = await _uow.Songs.GetFilteredSongsAsync(genreId, sortBy, descending, page, pageSize);
            var dtos = _mapper.Map<IEnumerable<SongDTO>>(pagedResult.Items);
            return new PagedResult<SongDTO>(dtos, pagedResult.TotalCount, page, pageSize);
        }

        public async Task<SongDTO?> GetByIdAsync(int id)
        {
            var song = await _uow.Songs.GetByIdAsync(id);
            return _mapper.Map<SongDTO?>(song);
        }

        public async Task<int> CreateAsync(SongDTO songDto)
        {
            var genres = new HashSet<Genre>();
            foreach (var gid in songDto.GenreIds.Distinct())
            {
                var genre = await _uow.Genres.GetByIdAsync(gid);
                if (genre != null) genres.Add(genre);
            }

            var song = new Song
            {
                Title = songDto.Title!,
                FilePath = songDto.FilePath!,
                ArtistId = songDto.ArtistId,
                UploadDate = DateTime.UtcNow,
                PlayCount = 0,
                Genres = genres
            };

            await _uow.Songs.AddAsync(song);
            var saved = await _uow.SaveChangesAsync();

            if (!saved)
            {
                throw new ValidationException("Не удалось сохранить песню в базе данных.", "");
            }

            return song.Id;
        }

        public async Task UpdateAsync(SongDTO songDto)
        {
            var song = await _uow.Songs.GetByIdAsync(songDto.Id);
            if (song == null)
                throw new ValidationException("Песня не найдена.", nameof(songDto.Id));

            song.Title = songDto.Title!;
            song.ArtistId = songDto.ArtistId;

            var genres = new HashSet<Genre>();
            foreach (var gid in songDto.GenreIds.Distinct())
            {
                var genre = await _uow.Genres.GetByIdAsync(gid);
                if (genre != null) genres.Add(genre);
            }
            song.Genres = genres;

            var updated = await _uow.Songs.UpdateAsync(song);
            if (!updated)
                throw new ValidationException("Не удалось сохранить изменения песни.", "");

            if (!await _uow.SaveChangesAsync())
                throw new ValidationException("Не удалось сохранить изменения песни.", "");
        }

        public async Task<string?> DeleteAsync(int id)
        {
            var info = await _uow.Songs.GetFileInfoAsync(id);
            var staged = await _uow.Songs.DeleteAsync(id);
            if (!staged)
            {
                throw new ValidationException("Песня не найдена.", nameof(id));
            }

            var saved = await _uow.SaveChangesAsync();
            if (!saved)
            {
                throw new ValidationException("Не удалось удалить песню.", nameof(id));
            }

            return info?.FilePath;
        }

        public Task IncrementPlayCountAsync(int id) => _uow.Songs.IncrementPlayCountAsync(id);

        public Task<(string FilePath, string Title)?> GetFileInfoAsync(int id) => _uow.Songs.GetFileInfoAsync(id);
    }
}
