using System.Security.Cryptography;
using System.Text;

namespace SIMS.Helpers
{
    /// <summary>
    /// Utility class for hashing and verifying passwords using SHA512.
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Hashes a plain-text password using SHA512 and encodes it in Base64.
        /// </summary>
        /// <param name="password">The plain-text password.</param>
        /// <returns>The hashed password as a Base64 string.</returns>
        public static string HashPassword(string password)
        {
            using var sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha512.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Verifies whether the entered password matches the stored hashed password.
        /// </summary>
        /// <param name="enteredPassword">The password entered by the user.</param>
        /// <param name="storedHash">The hashed password stored in the database.</param>
        /// <returns>True if the password matches; otherwise, false.</returns>
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = HashPassword(enteredPassword);
            return storedHash == enteredHash;
        }
    }
}
