namespace FSI.CloudShopping.WebAPI.Controllers.BackOffice;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Commands.Order;
using FSI.CloudShopping.Application.Commands.Payment;
using FSI.CloudShopping.Application.Queries.Order;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.WebAPI.Controllers;

[ApiController]
[Route("api/v1/backoffice/orders")]
[Authorize(Policy = "BackOfficePolicy")]
public class OrdersBackOfficeController : BaseApiController
{
    private readonly IMediator _mediator;

    public OrdersBackOfficeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>GET /api/v1/backoffice/orders — Lista paginada com filtros avançados.</summary>
    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? status = null,
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        OrderStatus? parsedStatus = status != null && Enum.TryParse<OrderStatus>(status, true, out var s) ? s : null;
        var query = new GetOrdersPagedQuery(page, pageSize, parsedStatus, dateFrom, dateTo, search);
        var result = await _mediator.Send(query, ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/backoffice/orders/{orderNumber} — Detalhe completo do pedido.</summary>
    [HttpGet("{orderNumber}")]
    public async Task<IActionResult> GetOrder(string orderNumber, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderByNumberQuery(orderNumber, null), ct);
        return HandleResult(result);
    }

    /// <summary>PUT /api/v1/backoffice/orders/{orderNumber}/status — Atualiza status do pedido.</summary>
    [HttpPut("{orderNumber}/status")]
    public async Task<IActionResult> UpdateStatus(string orderNumber,
        [FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
    {
        var command = new UpdateOrderStatusCommand(orderNumber, request.Status, request.TrackingNumber, request.Notes);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/backoffice/orders/{orderNumber}/payment/manual — Registra pagamento manual.</summary>
    [HttpPost("{orderNumber}/payment/manual")]
    public async Task<IActionResult> RegisterManualPayment(string orderNumber,
        [FromBody] ManualPaymentRequest request, CancellationToken ct)
    {
        var operatorId = GetCurrentCustomerId().ToString();
        var command = new ProcessManualPaymentCommand(
            orderNumber,
            Enum.Parse<PaymentMethod>(request.PaymentMethod, true),
            request.Notes,
            operatorId);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/backoffice/orders/{orderNumber}/payment/{paymentId}/confirm — Confirma pagamento manual.</summary>
    [HttpPost("{orderNumber}/payment/{paymentId:int}/confirm")]
    public async Task<IActionResult> ConfirmPayment(string orderNumber, int paymentId, CancellationToken ct)
    {
        var operatorId = GetCurrentCustomerId().ToString();
        var command = new ConfirmManualPaymentCommand(paymentId, operatorId);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }
}
