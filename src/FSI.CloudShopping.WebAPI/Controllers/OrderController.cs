namespace FSI.CloudShopping.WebAPI.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Commands.Order;
using FSI.CloudShopping.Application.Commands.Payment;
using FSI.CloudShopping.Application.Queries.Order;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Enums;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class OrderController : BaseApiController
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>POST /api/v1/orders/checkout — Cria pedido a partir do carrinho.</summary>
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] PlaceOrderRequest request, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var command = new PlaceOrderCommand(
            customerId, request.ShippingAddressId, request.Items,
            request.ShippingCost, request.CouponCode, request.Notes);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/orders/{orderNumber} — Detalhe de um pedido.</summary>
    [HttpGet("{orderNumber}")]
    public async Task<IActionResult> GetOrder(string orderNumber, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var result = await _mediator.Send(new GetOrderByNumberQuery(orderNumber, customerId), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/orders/{orderNumber}/cancel — Cancela pedido.</summary>
    [HttpPost("{orderNumber}/cancel")]
    public async Task<IActionResult> CancelOrder(string orderNumber, [FromBody] CancelOrderRequest request, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var result = await _mediator.Send(
            new CancelOrderCommand(orderNumber, customerId, request.Reason), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/orders/{orderNumber}/payment — Processa pagamento de um pedido.</summary>
    [HttpPost("{orderNumber}/payment")]
    public async Task<IActionResult> ProcessPayment(string orderNumber,
        [FromBody] ProcessPaymentRequest request, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var command = new ProcessPaymentCommand(
            orderNumber, customerId,
            Enum.Parse<PaymentMethod>(request.PaymentMethod, true),
            request.CardToken, request.InstallmentCount);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }
}
