using MusicPortal.BusinessLogic.DTO;

namespace MusicPortal.BusinessLogic.Services
{
    public interface IAccountService
    {
        Task<bool> RegisterAsync(RegisterDTO registerDto);
        Task<UserDTO> ValidateLoginAsync(string email, string password);
    }
}
