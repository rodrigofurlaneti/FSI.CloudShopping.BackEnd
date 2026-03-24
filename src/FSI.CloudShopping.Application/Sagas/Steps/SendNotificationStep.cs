namespace FSI.CloudShopping.Application.Sagas.Steps;

using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Sagas;
using Microsoft.Extensions.Logging;

/// <summary>
/// SAGA Step 4 — Send Notification.
/// Sends an order confirmation email to the customer.
/// Compensation: no-op (notification delivery failure is non-critical; retry via background job).
/// </summary>
public sealed class SendNotificationStep : ISagaStep<OrderCheckoutSagaState>
{
    private readonly IEmailService _emailService;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<SendNotificationStep> _logger;

    public string StepName => "SendNotification";

    public SendNotificationStep(
        IEmailService emailService,
        ICustomerRepository customerRepository,
        ILogger<SendNotificationStep> logger)
    {
        _emailService = emailService;
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(OrderCheckoutSagaState state, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(state.CustomerId, cancellationToken);
        if (customer is null)
        {
            _logger.LogWarning("Customer {CustomerId} not found, skipping notification.", state.CustomerId);
            return;
        }

        await _emailService.SendOrderConfirmationAsync(
            toEmail: customer.Email.Value,
            orderNumber: state.OrderNumber!,
            totalAmount: state.Items.Sum(i => i.UnitPrice * i.Quantity),
            cancellationToken: cancellationToken);

        state.NotificationSent = true;
        _logger.LogInformation("Order confirmation email sent for order {OrderNumber}", state.OrderNumber);
    }

    /// <summary>
    /// Notifications are idempotent and do not need compensation.
    /// Failed notifications are retried asynchronously via background job / outbox pattern.
    /// </summary>
    public Task CompensateAsync(OrderCheckoutSagaState state, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Notification compensation skipped for SAGA {SagaId} — non-critical step.", state.SagaId);
        return Task.CompletedTask;
    }
}
