using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public SongService(ISongRepository songRepository, IGenreRepository genreRepository, IMapper mapper)
        {
            _songRepository = songRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<SongDTO>> GetFilteredSongsAsync(int? genreId, string? sortBy, bool descending, int page, int pageSize)
        {
            var pagedResult = await _songRepository.GetFilteredSongsAsync(genreId, sortBy, descending, page, pageSize);
            var dtos = _mapper.Map<IEnumerable<SongDTO>>(pagedResult.Items);
            return new PagedResult<SongDTO>(dtos, pagedResult.TotalCount, page, pageSize);
        }

        public async Task<SongDTO?> GetByIdAsync(int id)
        {
            var song = await _songRepository.GetByIdAsync(id);
            return _mapper.Map<SongDTO?>(song);
        }

        public async Task<int> CreateAsync(SongDTO songDto)
        {
            var genres = new HashSet<Genre>();
            foreach (var gid in songDto.Genres.Select(g => g.Id).Distinct())
            {
                var genre = await _genreRepository.GetByIdAsync(gid);
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

            var saved = await _songRepository.AddAsync(song);
            if (!saved)
                throw new ValidationException("Не удалось сохранить песню в базе данных.", "");

            return song.Id;
        }

        public async Task UpdateAsync(SongDTO songDto)
        {
            var song = await _songRepository.GetByIdAsync(songDto.Id);
            if (song == null)
                throw new ValidationException("Песня не найдена.", nameof(songDto.Id));

            song.Title = songDto.Title!;
            song.ArtistId = songDto.ArtistId;

            var genres = new HashSet<Genre>();
            foreach (var gid in songDto.Genres.Select(g => g.Id).Distinct())
            {
                var genre = await _genreRepository.GetByIdAsync(gid);
                if (genre != null) genres.Add(genre);
            }
            song.Genres = genres;

            var updated = await _songRepository.UpdateAsync(song);
            if (!updated)
                throw new ValidationException("Не удалось сохранить изменения песни.", "");
        }

        public async Task<string?> DeleteAsync(int id)
        {
            var info = await _songRepository.GetFileInfoAsync(id);

            var deleted = await _songRepository.DeleteAsync(id);
            if (!deleted)
                throw new ValidationException("Песня не найдена.", nameof(id));

            return info?.FilePath;
        }

        public Task IncrementPlayCountAsync(int id) => _songRepository.IncrementPlayCountAsync(id);

        public Task<(string FilePath, string Title)?> GetFileInfoAsync(int id) => _songRepository.GetFileInfoAsync(id);
    }
}