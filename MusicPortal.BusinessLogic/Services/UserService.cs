using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetPendingUsersAsync()
        {
            var pending = await _uow.Users.GetPendingUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(pending);
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var user = await _uow.Users.GetByIdAsync(id);
            return _mapper.Map<UserDTO?>(user);
        }

        public async Task ApproveAsync(int userId)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ValidationException("Пользователь не найден.", nameof(userId));

            var userRole = await _uow.Roles.GetByNameAsync("User");
            if (userRole == null)
                throw new ValidationException("Роль 'User' не настроена в системе.", "");

            var roles = new List<Role> { userRole };

            var artistProfile = await _uow.Artists.GetByUserIdAsync(userId);
            if (artistProfile != null)
            {
                var artistRole = await _uow.Roles.GetByNameAsync("Artist");
                if (artistRole != null) roles.Add(artistRole);
            }

            var success = await _uow.Users.ApproveUserAsync(user, roles);
            if (!success)
                throw new ValidationException("Не удалось сохранить изменения при одобрении пользователя.", "");
        }

        public async Task RejectAsync(int userId)
        {
            var staged = await _uow.Users.DeleteAsync(userId);
            if (!staged)
                throw new ValidationException("Заявка пользователя не найдена.", nameof(userId));

            if (!await _uow.SaveChangesAsync())
                throw new ValidationException("Не удалось отклонить заявку.", "");
        }

        public async Task DeleteAsync(int userId)
        {
            var staged = await _uow.Users.DeleteAsync(userId);
            if (!staged)
                throw new ValidationException("Пользователь не найден.", nameof(userId));

            if (!await _uow.SaveChangesAsync())
                throw new ValidationException("Не удалось удалить пользователя.", "");
        }

        public async Task<PagedResult<UserDTO>> GetAllUsersAsync(int page, int pageSize)
        {
            var paged = await _uow.Users.GetAllUsersAsync(page, pageSize);
            var dtos = _mapper.Map<List<UserDTO>>(paged.Items);

            return new PagedResult<UserDTO>
            {
                Items = dtos,
                TotalCount = paged.TotalCount,
                Page = paged.Page,
                PageSize = paged.PageSize
            };
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            var roles = await _uow.Roles.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }

        public async Task UpdateUserAsync(int userId, List<int> roleIds, bool isApproved)
        {
            var user = await _uow.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ValidationException("Пользователь не найден.", nameof(userId));

            var roles = await _uow.Roles.GetByIdsAsync(roleIds);
            user.IsApproved = isApproved;

            var success = await _uow.Users.SetRolesAsync(user, roles);
            if (!success)
                throw new ValidationException("Не удалось обновить пользователя.", "");
        }
    }
}