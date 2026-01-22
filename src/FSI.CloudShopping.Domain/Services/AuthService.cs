using FSI.CloudShopping.Domain.Interfaces.Services;
namespace FSI.CloudShopping.Domain.Services
{
    public class AuthService : IAuthService
    {
        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha não pode ser vazia.");
            return password;
        }
        public bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
                return false;
            return password == hash; 
        }
    }
}
