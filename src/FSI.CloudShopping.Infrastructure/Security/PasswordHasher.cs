using BCrypt.Net;
using FSI.CloudShopping.Domain.Interfaces;

namespace FSI.CloudShopping.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 12;
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser vazia.");

            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }
        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}