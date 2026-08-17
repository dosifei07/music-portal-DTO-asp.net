using AutoMapper;
using MusicPortal.BusinessLogic.DTO;
using MusicPortal.BusinessLogic.Infrastructure;
using MusicPortal.DataAccess.Models;
using MusicPortal.DataAccess.Repositories.Interfaces;

namespace MusicPortal.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> RegisterAsync(RegisterDTO registerDto)
        {
            var existing = await _uow.Users.GetByEmailAsync(registerDto.Email);
            if (existing != null)
                throw new ValidationException("Пользователь с таким Email уже зарегистрирован", nameof(registerDto.Email));

            var isFirstUser = !await _uow.Users.AnyUsersExistAsync();
            var roleName = isFirstUser ? "Admin" : "Pending";

            var role = await _uow.Roles.GetByNameAsync(roleName);
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

            await _uow.Users.AddAsync(newUser);
            if (!await _uow.SaveChangesAsync())
                throw new ValidationException("Не удалось создать учётную запись. Попробуйте ещё раз.", "");

            if (registerDto.IsArtistRequested)
            {
                var newArtist = new Artist
                {
                    Name = string.IsNullOrWhiteSpace(registerDto.ArtistName) ? registerDto.Username : registerDto.ArtistName!,
                    Bio = registerDto.Bio ?? string.Empty,
                    UserId = newUser.Id
                };
                await _uow.Artists.AddAsync(newArtist);
                await _uow.SaveChangesAsync();
            }

            return isFirstUser;
        }

        public async Task<UserDTO> ValidateLoginAsync(string email, string password)
        {
            var user = await _uow.Users.GetByEmailAsync(email);
            if (user == null || !PasswordHashHelper.VerifyPassword(user.PasswordHash, password))
                throw new ValidationException("Неверный Email или пароль", "");

            if (!user.IsApproved)
                throw new ValidationException("Ваша учетная запись еще не активирована администратором.", "");

            return _mapper.Map<UserDTO>(user);
        }
    }
}