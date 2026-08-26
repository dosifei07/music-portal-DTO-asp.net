using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<CommentDTO>> GetBySongIdAsync(int songId, int page = 1, int pageSize = 10)
        {
            var pagedComments = await _commentRepository.GetBySongIdAsync(songId, page, pageSize);
            var mappedItems = _mapper.Map<List<CommentDTO>>(pagedComments.Items);

            return new PagedResult<CommentDTO>
            {
                Items = mappedItems,
                TotalCount = pagedComments.TotalCount,
                Page = pagedComments.Page,
                PageSize = pagedComments.PageSize
            };
        }

        public async Task<IEnumerable<CommentDTO>> GetAllAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public async Task<CommentDTO?> GetByIdAsync(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return _mapper.Map<CommentDTO?>(comment);
        }

        public async Task AddAsync(CommentDTO commentDto)
        {
            await _commentRepository.AddAsync(commentDto.SongId, commentDto.UserId, commentDto.Text ?? string.Empty);
        }

        public async Task AddAsync(int songId, int userId, string text)
        {
            await _commentRepository.AddAsync(songId, userId, text);
        }

        public async Task UpdateAsync(int id, string text)
        {
            var updated = await _commentRepository.UpdateAsync(id, text);
            if (!updated)
                throw new ValidationException("Комментарий не найден.", nameof(id));
        }

        public async Task DeleteAsync(int id)
        {
            var deleted = await _commentRepository.DeleteAsync(id);
            if (!deleted)
                throw new ValidationException("Комментарий не найден.", nameof(id));
        }
    }
}