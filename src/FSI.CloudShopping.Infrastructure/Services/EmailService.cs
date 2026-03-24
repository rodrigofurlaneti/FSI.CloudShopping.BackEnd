namespace FSI.CloudShopping.Infrastructure.Services;

using FSI.CloudShopping.Application.Interfaces;
using Microsoft.Extensions.Logging;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendWelcomeEmailAsync(string email, string name, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending welcome email to {Email}", email);
        // TODO: Implement actual email sending using SMTP
        await Task.CompletedTask;
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetToken, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending password reset email to {Email}", email);
        // TODO: Implement actual email sending using SMTP
        await Task.CompletedTask;
    }

    public async Task SendOrderConfirmationEmailAsync(string email, string orderNumber, decimal totalAmount, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending order confirmation email to {Email} for order {OrderNumber}", email, orderNumber);
        // TODO: Implement actual email sending using SMTP
        await Task.CompletedTask;
    }

    public async Task SendShippingNotificationEmailAsync(string email, string orderNumber, string trackingNumber, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending shipping notification email to {Email} for order {OrderNumber}", email, orderNumber);
        // TODO: Implement actual email sending using SMTP
        await Task.CompletedTask;
    }

    public async Task SendRefundNotificationEmailAsync(string email, string orderNumber, decimal refundAmount, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending refund notification email to {Email} for order {OrderNumber}", email, orderNumber);
        // TODO: Implement actual email sending using SMTP
        await Task.CompletedTask;
    }

    public async Task SendPaymentFailedEmailAsync(string email, string orderNumber, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Sending payment failed email to {Email} for order {OrderNumber}", email, orderNumber);
        await Task.CompletedTask;
    }

    /// <summary>Alias used by the SAGA SendNotificationStep.</summary>
    public Task SendOrderConfirmationAsync(string toEmail, string orderNumber, decimal totalAmount, CancellationToken cancellationToken = default)
        => SendOrderConfirmationEmailAsync(toEmail, orderNumber, totalAmount, cancellationToken);
}
