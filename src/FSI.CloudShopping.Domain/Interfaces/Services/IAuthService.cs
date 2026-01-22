namespace FSI.CloudShopping.Domain.Interfaces.Services
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
