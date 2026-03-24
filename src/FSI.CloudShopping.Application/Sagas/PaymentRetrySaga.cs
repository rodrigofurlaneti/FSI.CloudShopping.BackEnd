namespace FSI.CloudShopping.Application.Sagas;

using MediatR;
using Microsoft.Extensions.Logging;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Events;
using FSI.CloudShopping.Domain.Interfaces;

/// <summary>
/// SAGA de Retry de Pagamento com Backoff Exponencial.
///
/// Política de retry:
///   - Tentativa 1: imediato
///   - Tentativa 2: aguarda 1 minuto
///   - Tentativa 3: aguarda 5 minutos
///   - Após 3 falhas: notifica cliente e encerra
///
/// Em produção, os delays devem ser implementados via job scheduler (Hangfire/Quartz).
/// Aqui registramos a intenção e o estado para o scheduler executar.
/// </summary>
public class PaymentRetrySaga : INotificationHandler<PaymentFailedEvent>
{
    private const int MaxRetries = 3;
    private static readonly TimeSpan[] RetryDelays =
    [
        TimeSpan.Zero,
        TimeSpan.FromMinutes(1),
        TimeSpan.FromMinutes(5)
    ];

    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PaymentRetrySaga> _logger;

    public PaymentRetrySaga(
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IEmailService emailService,
        IUnitOfWork unitOfWork,
        ILogger<PaymentRetrySaga> logger)
    {
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(PaymentFailedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("[SAGA:Retry] PaymentFailed — PaymentId: {PaymentId}, RetryCount: {RetryCount}",
            notification.PaymentId, notification.RetryCount);

        if (notification.RetryCount >= MaxRetries)
        {
            _logger.LogError("[SAGA:Retry] Max retries reached for PaymentId {PaymentId}. Notifying customer.",
                notification.PaymentId);

            await NotifyCustomerPaymentFailed(notification.OrderId, cancellationToken);
            return;
        }

        var delay = RetryDelays[Math.Min(notification.RetryCount, RetryDelays.Length - 1)];
        var scheduledAt = DateTime.UtcNow.Add(delay);

        _logger.LogInformation(
            "[SAGA:Retry] Scheduling retry #{Attempt} for PaymentId {PaymentId} at {ScheduledAt}",
            notification.RetryCount + 1, notification.PaymentId, scheduledAt);

        // In production: enqueue a background job via Hangfire/Quartz with the delay
        // BackgroundJob.Schedule(() => RetryPayment(notification.PaymentId), delay);
        // For now, we log the intent — the payment status is 'Failed' with retry count tracked
    }

    private async Task NotifyCustomerPaymentFailed(int orderId, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
            if (order?.Customer?.Email?.Value != null)
            {
                await _emailService.SendPaymentFailedEmailAsync(
                    order.Customer.Email.Value,
                    order.OrderNumber,
                    cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SAGA:Retry] Failed to send payment failure notification for OrderId {OrderId}", orderId);
        }
    }
}
