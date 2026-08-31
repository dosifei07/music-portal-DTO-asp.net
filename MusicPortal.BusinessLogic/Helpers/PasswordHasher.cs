using Microsoft.AspNetCore.Identity;

namespace MusicPortal.BusinessLogic.Infrastructure
{
    public static class PasswordHashHelper
    {
        private static readonly PasswordHasher<object> _hasher = new();
        private static readonly object _dummyUser = new();

        public static string HashPassword(string password)
            => _hasher.HashPassword(_dummyUser, password);

        public static bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
                return false;

            var result = _hasher.VerifyHashedPassword(_dummyUser, hashedPassword, providedPassword);

            return result != PasswordVerificationResult.Failed;
        }
    }
}