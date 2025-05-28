using BCrypt.Net;

namespace InventoryControl.Infrastructure.Security
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.EnhancedHashPassword(password, HashType.SHA512);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.EnhancedVerify(password, hash, HashType.SHA512);
        }
    }
}