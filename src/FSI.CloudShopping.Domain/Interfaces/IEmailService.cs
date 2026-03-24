namespace FSI.CloudShopping.Domain.Interfaces;

/// <summary>
/// Domain-level email service contract.
/// Used by domain services and command handlers that only need email primitives.
/// Full contract is in FSI.CloudShopping.Application.Interfaces.IEmailService.
/// </summary>
public interface IEmailService
{
    Task SendResetPasswordEmailAsync(string email, string newPassword, CancellationToken cancellationToken = default);
    Task SendPasswordResetEmailAsync(string email, string resetToken, CancellationToken cancellationToken = default);
    Task SendOrderConfirmationEmailAsync(string email, string orderNumber, decimal totalAmount, CancellationToken cancellationToken = default);
    Task SendPaymentFailedEmailAsync(string email, string orderNumber, CancellationToken cancellationToken = default);
    Task SendWelcomeEmailAsync(string email, string name, CancellationToken cancellationToken = default);
}
