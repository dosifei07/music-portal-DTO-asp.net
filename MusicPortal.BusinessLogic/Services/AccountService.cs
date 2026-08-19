using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public AccountService(IUserRepository userRepository, IRoleRepository roleRepository, IArtistRepository artistRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDto)
        {
            var existing = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existing != null)
                throw new ValidationException("Пользователь с таким Email уже зарегистрирован", nameof(registerDto.Email));

            var isFirstUser = !await _userRepository.AnyUsersExistAsync();
            var roleName = isFirstUser ? "Admin" : "Pending";

            var role = await _roleRepository.GetByNameAsync(roleName);
            if (role == null)
                throw new ValidationException($"Роль '{roleName}' не настроена в системе.", "");

            var newUser = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = PasswordHashHelper.HashPassword(registerDto.Password),
                IsApproved = isFirstUser,
                CreatedAt = DateTime.UtcNow,
                Roles = new List<Role> { role }
            };

            var saved = await _userRepository.AddAsync(newUser);
            if (!saved)
                throw new ValidationException("Не удалось создать учётную запись. Попробуйте ещё раз.", "");

            if (registerDto.IsArtistRequested)
            {
                var newArtist = new Artist
                {
                    Name = string.IsNullOrWhiteSpace(registerDto.ArtistName) ? registerDto.Username : registerDto.ArtistName!,
                    Bio = registerDto.Bio ?? string.Empty,
                    User = newUser
                };
                await _artistRepository.AddAsync(newArtist);
            }

            return isFirstUser;
        }

        public async Task<UserDTO> ValidateLoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !PasswordHashHelper.VerifyPassword(user.PasswordHash, password))
                throw new ValidationException("Неверный Email или пароль", "");

            if (!user.IsApproved)
                throw new ValidationException("Ваша учетная запись еще не активирована администратором.", "");

            return _mapper.Map<UserDTO>(user);
        }
    }
}