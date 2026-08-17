using Microsoft.AspNetCore.Identity;

namespace MusicPortal.BusinessLogic.Infrastructure
{
    public static class PasswordHashHelper
    {
        private static readonly PasswordHasher<object> _hasher = new();

        public static string HashPassword(string password) => _hasher.HashPassword(new object(), password);

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
            => _hasher.VerifyHashedPassword(new object(), hashedPassword, providedPassword) == PasswordVerificationResult.Success;
    }
}