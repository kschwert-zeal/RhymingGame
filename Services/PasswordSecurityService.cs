using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace RhymingGame.Services
{
    public class PasswordSecurityService
    {
        private const int SaltSize = 32; // 256 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 100000; // PBKDF2 iterations (recommended minimum)

        /// <summary>
        /// Creates a secure password hash with salt
        /// </summary>
        public (string hash, string salt) HashPassword(string password)
        {
            // Generate a random salt
            byte[] saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            // Hash the password with PBKDF2
            string hash = HashPasswordWithSalt(password, saltBytes);

            return (hash, salt);
        }

        /// <summary>
        /// Verifies a password against the stored hash and salt
        /// </summary>
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            try
            {
                byte[] saltBytes = Convert.FromBase64String(storedSalt);
                string computedHash = HashPasswordWithSalt(password, saltBytes);

                // Use constant-time comparison to prevent timing attacks
                return SlowEquals(computedHash, storedHash);
            }
            catch
            {
                return false;
            }
        }

        private string HashPasswordWithSalt(string password, byte[] salt)
        {
            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Constant-time string comparison to prevent timing attacks
        /// </summary>
        private bool SlowEquals(string a, string b)
        {
            if (a == null || b == null || a.Length != b.Length)
                return false;

            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }

        /// <summary>
        /// Validates password strength
        /// </summary>
        public (bool isValid, string errorMessage) ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
                return (false, "Password cannot be empty.");

            if (password.Length < 8)
                return (false, "Password must be at least 8 characters long.");

            if (password.Length > 128)
                return (false, "Password cannot exceed 128 characters.");

            bool hasUpper = false, hasLower = false, hasDigit = false, hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecial = true;
            }

            if (!hasUpper)
                return (false, "Password must contain at least one uppercase letter.");
            if (!hasLower)
                return (false, "Password must contain at least one lowercase letter.");
            if (!hasDigit)
                return (false, "Password must contain at least one number.");
            if (!hasSpecial)
                return (false, "Password must contain at least one special character.");

            return (true, string.Empty);
        }
    }
}
