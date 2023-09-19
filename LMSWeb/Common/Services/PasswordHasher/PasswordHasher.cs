using System.Security.Cryptography;

namespace LMSWeb.Common.Services.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int _saltSize = 16;
        private const int _hashSize = 32;
        private const int _iterations = 10000;
        private const string _delimiter = ";";
        private readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(_saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithmName, _hashSize);

            return String.Join(_delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string hashedPassword, string password)
        {
            var elements = hashedPassword.Split(_delimiter);
            var salt = Convert.FromBase64String(elements[0]);
            var hash = Convert.FromBase64String(elements[1]);
            var newHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithmName, _hashSize);

            return CryptographicOperations.FixedTimeEquals(hash, newHash);
        }
    }
}
