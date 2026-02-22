namespace FSI.CloudShopping.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string email, string newPassword);
    }
}
