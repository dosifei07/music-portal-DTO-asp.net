using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GenreDTO>> GetAllAsync()
        {
            var genres = await _uow.Genres.GetAllAsync();
            return _mapper.Map<IEnumerable<GenreDTO>>(genres);
        }

        public async Task<GenreDTO?> GetByIdAsync(int id)
        {
            var genre = await _uow.Genres.GetByIdAsync(id);
            return _mapper.Map<GenreDTO?>(genre);
        }

        public async Task<int> CreateAsync(GenreDTO dto)
        {
            var genre = new Genre { Name = dto.Name };

            var added = await _uow.Genres.AddAsync(genre);
            if (!added)
                throw new ValidationException("Не удалось сохранить жанр в базе данных.", "");

            return genre.Id;
        }

        public async Task UpdateAsync(GenreDTO dto)
        {
            var genre = await _uow.Genres.GetByIdAsync(dto.Id);
            if (genre == null)
                throw new ValidationException("Жанр не найден.", nameof(dto.Id));

            genre.Name = dto.Name;

            var updated = await _uow.Genres.UpdateAsync(genre);
            if (!updated)
                throw new ValidationException("Не удалось сохранить изменения жанра.", "");
        }

        public async Task DeleteAsync(int id)
        {
            var deleted = await _uow.Genres.DeleteAsync(id);
            if (!deleted)
                throw new ValidationException("Жанр не найден.", nameof(id));
        }
    }
}