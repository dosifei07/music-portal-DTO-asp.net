using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
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

        public async Task AddAsync(CommentDTO commentDto)
        {
            await _commentRepository.AddAsync(commentDto.SongId, commentDto.UserId, commentDto.Text ?? string.Empty);
        }

        public async Task AddAsync(int songId, int userId, string text)
        {
            await _commentRepository.AddAsync(songId, userId, text);
        }
    }
}