namespace FSI.CloudShopping.Application.Interfaces;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email, string name, CancellationToken cancellationToken = default);
    Task SendPasswordResetEmailAsync(string email, string resetToken, CancellationToken cancellationToken = default);
    Task SendOrderConfirmationEmailAsync(string email, string orderNumber, decimal totalAmount, CancellationToken cancellationToken = default);
    Task SendShippingNotificationEmailAsync(string email, string orderNumber, string trackingNumber, CancellationToken cancellationToken = default);
    Task SendRefundNotificationEmailAsync(string email, string orderNumber, decimal refundAmount, CancellationToken cancellationToken = default);
    Task SendPaymentFailedEmailAsync(string email, string orderNumber, CancellationToken cancellationToken = default);
}
