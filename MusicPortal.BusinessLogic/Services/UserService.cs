using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository, IArtistRepository artistRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetPendingUsersAsync()
        {
            var pending = await _userRepository.GetPendingUsersAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(pending);
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO?>(user);
        }

        public async Task ApproveAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ValidationException("Пользователь не найден.", nameof(userId));

            var userRole = await _roleRepository.GetByNameAsync("User");
            if (userRole == null)
                throw new ValidationException("Роль 'User' не настроена в системе.", "");

            var roles = new List<Role> { userRole };

            var artistProfile = await _artistRepository.GetByUserIdAsync(userId);
            if (artistProfile != null)
            {
                var artistRole = await _roleRepository.GetByNameAsync("Artist");
                if (artistRole != null) roles.Add(artistRole);
            }

            var success = await _userRepository.ApproveUserAsync(user, roles);
            if (!success)
                throw new ValidationException("Не удалось сохранить изменения при одобрении пользователя.", "");
        }

        public async Task RejectAsync(int userId)
        {
            var staged = await _userRepository.DeleteAsync(userId);
            if (!staged)
                throw new ValidationException("Заявка пользователя не найдена.", nameof(userId));
        }

        public async Task DeleteAsync(int userId)
        {
            var staged = await _userRepository.DeleteAsync(userId);
            if (!staged)
                throw new ValidationException("Пользователь не найден.", nameof(userId));
        }

        public async Task<PagedResult<UserDTO>> GetAllUsersAsync(int page, int pageSize)
        {
            var paged = await _userRepository.GetAllUsersAsync(page, pageSize);
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
            var roles = await _roleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RoleDTO>>(roles);
        }

        public async Task UpdateUserAsync(int userId, List<int> roleIds, bool isApproved)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ValidationException("Пользователь не найден.", nameof(userId));

            var roles = await _roleRepository.GetByIdsAsync(roleIds);
            user.IsApproved = isApproved;

            var success = await _userRepository.SetRolesAsync(user, roles);
            if (!success)
                throw new ValidationException("Не удалось обновить пользователя.", "");
        }
    }
}