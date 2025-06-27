using System.Security.Cryptography;
using System.Text;

namespace SIMS.Helpers
{
    public static class PasswordHelper
    {
        // ✅ Mã hóa mật khẩu dùng SHA512 + Base64
        public static string HashPassword(string password)
        {
            using var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha512.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // ✅ So sánh mật khẩu nhập vào với hash lưu trong DB
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            var enteredHash = HashPassword(enteredPassword);
            return storedHash == enteredHash;
        }
    }
}
