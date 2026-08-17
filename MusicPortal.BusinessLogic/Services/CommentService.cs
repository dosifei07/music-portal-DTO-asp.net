using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedResult<CommentDTO>> GetBySongIdAsync(int songId, int page = 1, int pageSize = 10)
        {
            var pagedComments = await _uow.Comments.GetBySongIdAsync(songId, page, pageSize);
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
            await _uow.Comments.AddAsync(commentDto.SongId, commentDto.UserId, commentDto.Text ?? string.Empty);
            await _uow.SaveChangesAsync();
        }

        public async Task AddAsync(int songId, int userId, string text)
        {
            await _uow.Comments.AddAsync(songId, userId, text);
            await _uow.SaveChangesAsync();
        }
    }
}