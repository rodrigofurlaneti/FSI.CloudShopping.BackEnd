namespace FSI.CloudShopping.WebAPI.Controllers;

using FSI.CloudShopping.Application.Commands.Order;
using FSI.CloudShopping.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Exposes the Order Checkout endpoint.
/// Internally triggers the OrderCheckout SAGA (DDD + CQRS + SAGA pattern).
///
/// POST /api/checkout
/// The request is forwarded as a CheckoutOrderCommand via MediatR.
/// The SAGA orchestrates: Reserve Stock → Process Payment → Confirm Order → Send Notification.
/// On any step failure, compensating transactions restore system consistency.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class CheckoutController : ControllerBase
{
    private readonly IMediator _mediator;

    public CheckoutController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Initiates a checkout using the SAGA pattern.
    /// Returns 200 with order details on success, or 422 with the failure reason on compensation.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CheckoutOrderResult), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 422)]
    public async Task<IActionResult> Checkout([FromBody] CheckoutOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        return result switch
        {
            { IsSuccess: true } r => Ok(((Result<CheckoutOrderResult>.Success)r).Value),
            { IsFailure: true } r => UnprocessableEntity(new ProblemDetails
            {
                Title = "Checkout failed",
                Detail = ((Result<CheckoutOrderResult>.Failure)r).Errors.FirstOrDefault(),
                Status = 422
            }),
            _ => StatusCode(500)
        };
    }
}
