using ProjectChapeau.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ProjectChapeau.Services
{
    public class PasswordService : IPasswordService
    {
        private const int byteSize = 16;

        public string GenerateSalt()
        {
            byte[] saltBytes = new byte[byteSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string saltString = Convert.ToBase64String(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public string HashPassword(string input)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashBytes);
            }
        }

        public string InterleaveSalt(string password, string salt)
        {
            StringBuilder interleaved = new StringBuilder();
            int saltIndex = 0;

            for (int i = 0; i < password.Length; i++)
            {
                interleaved.Append(password[i]);
                if (saltIndex < salt.Length)
                {
                    interleaved.Append(salt[saltIndex]);
                    saltIndex++;
                }
            }

            // Append remaining salt if any
            if (saltIndex < salt.Length)
                interleaved.Append(salt.Substring(saltIndex));

            return interleaved.ToString();
        }
    }
}
