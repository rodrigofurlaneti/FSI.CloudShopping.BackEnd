namespace FSI.CloudShopping.Application.Sagas;

using MediatR;
using Microsoft.Extensions.Logging;
using FSI.CloudShopping.Application.Commands.Order;
using FSI.CloudShopping.Application.Commands.Payment;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.Events;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Services;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// SAGA de Processamento de Pedido.
///
/// Fluxo:
///   [OrderPlacedEvent]
///     → Step 1: ReserveStock        (compensação: ReleaseStock)
///     → Step 2: ProcessPayment      (compensação: ReleaseStock + CancelOrder)
///     → Step 3: ConfirmOrder        ← sucesso final
///
/// Em caso de falha em qualquer step, as ações compensatórias são executadas
/// para garantir consistência eventual (eventual consistency).
/// </summary>
public class OrderProcessingSaga :
    INotificationHandler<OrderPlacedEvent>,
    INotificationHandler<PaymentCapturedEvent>,
    INotificationHandler<PaymentFailedEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IStockDomainService _stockService;
    private readonly IPaymentGatewayService _paymentGateway;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<OrderProcessingSaga> _logger;

    public OrderProcessingSaga(
        IOrderRepository orderRepository,
        IStockDomainService stockService,
        IPaymentGatewayService paymentGateway,
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork,
        ILogger<OrderProcessingSaga> logger)
    {
        _orderRepository = orderRepository;
        _stockService = stockService;
        _paymentGateway = paymentGateway;
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>Step 1 — Pedido criado: reservar estoque.</summary>
    public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[SAGA] OrderPlaced — OrderNumber: {OrderNumber}", notification.OrderNumber);

        var order = await _orderRepository.GetByOrderNumberAsync(notification.OrderNumber, cancellationToken);
        if (order == null)
        {
            _logger.LogError("[SAGA] Order not found: {OrderNumber}", notification.OrderNumber);
            return;
        }

        var stockItems = order.Items
            .Select(i => (i.ProductId, i.Quantity))
            .ToList();

        var reserved = await _stockService.ReserveStockAsync(stockItems, cancellationToken);

        if (!reserved)
        {
            _logger.LogWarning("[SAGA] Stock reservation failed — Cancelling order {OrderNumber}", notification.OrderNumber);
            await CompensateCancelOrder(order, "Estoque insuficiente.", cancellationToken);
            return;
        }

        _logger.LogInformation("[SAGA] Stock reserved for order {OrderNumber}", notification.OrderNumber);
        // Payment is initiated by the customer via PaymentController after order placement
    }

    /// <summary>Step 3 — Pagamento capturado: confirmar pedido.</summary>
    public async Task Handle(PaymentCapturedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[SAGA] PaymentCaptured — OrderId: {OrderId}", notification.OrderId);

        var order = await _orderRepository.GetByIdAsync(notification.OrderId, cancellationToken);
        if (order == null) return;

        if (order.Status == OrderStatus.Pending)
        {
            order.Confirm();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("[SAGA] Order {OrderNumber} confirmed.", order.OrderNumber);
        }
    }

    /// <summary>Compensação — Pagamento falhou: liberar estoque e cancelar pedido.</summary>
    public async Task Handle(PaymentFailedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("[SAGA] PaymentFailed — OrderId: {OrderId}, Reason: {Reason}",
            notification.OrderId, notification.Reason);

        var order = await _orderRepository.GetByIdAsync(notification.OrderId, cancellationToken);
        if (order == null) return;

        var stockItems = order.Items
            .Select(i => (i.ProductId, i.Quantity))
            .ToList();

        await _stockService.ReleaseStockAsync(stockItems, cancellationToken);
        await CompensateCancelOrder(order, $"Pagamento recusado: {notification.Reason}", cancellationToken);
    }

    private async Task CompensateCancelOrder(Order order, string reason, CancellationToken cancellationToken)
    {
        try
        {
            if (order.CanBeCancelled)
            {
                order.Cancel(reason);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("[SAGA] Order {OrderNumber} cancelled. Reason: {Reason}",
                    order.OrderNumber, reason);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[SAGA] Failed to cancel order {OrderNumber}", order.OrderNumber);
        }
    }
}
