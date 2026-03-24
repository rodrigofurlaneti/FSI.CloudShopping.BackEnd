namespace FSI.CloudShopping.WebAPI.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Commands.Cart;
using FSI.CloudShopping.Application.Queries.Cart;
using FSI.CloudShopping.Application.DTOs;

/// <summary>
/// Cart controller — suporta usuários anônimos (via sessionToken) e autenticados.
/// Frontend deve armazenar o sessionToken em localStorage e enviar via query string ou header.
/// Cache local do carrinho deve ser invalidado sempre que este endpoint retornar dados.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class CartController : BaseApiController
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>GET /api/v1/cart?token={sessionToken} — Retorna o carrinho atual.</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCart([FromQuery] Guid token, CancellationToken ct)
    {
        var query = new GetCartQuery(token);
        var result = await _mediator.Send(query, ct);

        Response.Headers["Cache-Control"] = "no-store";
        Response.Headers["ETag"] = $"\"{token}\"";

        return HandleResult(result);
    }

    /// <summary>POST /api/v1/cart/items — Adiciona item ao carrinho.</summary>
    [HttpPost("items")]
    [AllowAnonymous]
    public async Task<IActionResult> AddItem([FromQuery] Guid token, [FromBody] AddToCartRequest request, CancellationToken ct)
    {
        var command = new AddToCartCommand(
            token, request.ProductId, request.ProductName,
            request.ProductImageUrl, request.ProductSku, request.Quantity, request.UnitPrice);

        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>PUT /api/v1/cart/items/{productId} — Atualiza quantidade de um item.</summary>
    [HttpPut("items/{productId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateItem([FromQuery] Guid token, int productId,
        [FromBody] UpdateCartItemRequest request, CancellationToken ct)
    {
        var command = new UpdateCartItemCommand(token, productId, request.Quantity);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>DELETE /api/v1/cart/items/{productId} — Remove item do carrinho.</summary>
    [HttpDelete("items/{productId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> RemoveItem([FromQuery] Guid token, int productId, CancellationToken ct)
    {
        var command = new RemoveFromCartCommand(token, productId);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>DELETE /api/v1/cart — Limpa o carrinho.</summary>
    [HttpDelete]
    [AllowAnonymous]
    public async Task<IActionResult> ClearCart([FromQuery] Guid token, CancellationToken ct)
    {
        var command = new ClearCartCommand(token);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/cart/merge — Mescla carrinho de visitante com carrinho do usuário logado.</summary>
    [HttpPost("merge")]
    [Authorize]
    public async Task<IActionResult> MergeCart([FromBody] MergeCartsRequest request, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var command = new MergeCartsCommand(request.GuestToken, customerId);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }
}
