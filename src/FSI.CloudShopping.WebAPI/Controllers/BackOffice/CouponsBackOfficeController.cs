namespace FSI.CloudShopping.WebAPI.Controllers.BackOffice;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Commands.Coupon;
using FSI.CloudShopping.Application.Queries.Coupon;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.WebAPI.Controllers;

[ApiController]
[Route("api/v1/backoffice/coupons")]
[Authorize(Policy = "BackOfficePolicy")]
public class CouponsBackOfficeController : BaseApiController
{
    private readonly IMediator _mediator;

    public CouponsBackOfficeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>GET /api/v1/backoffice/coupons — Lista paginada de cupons.</summary>
    [HttpGet]
    public async Task<IActionResult> GetCoupons(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = new GetCouponsPagedQuery(page, pageSize);
        var result = await _mediator.Send(query, ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/backoffice/coupons/{id} — Detalhe do cupom.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCoupon(Guid id, CancellationToken ct)
    {
        var query = new GetCouponByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/backoffice/coupons — Cria novo cupom.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponRequest request, CancellationToken ct)
    {
        if (!Enum.TryParse<CouponDiscountType>(request.DiscountType, ignoreCase: true, out var discountType))
            return BadRequest(new { success = false, errors = new[] { "Tipo de desconto inválido. Use 'Percentage' ou 'Fixed'." } });

        var command = new CreateCouponCommand(
            request.Code,
            request.Description,
            discountType,
            request.DiscountValue,
            request.MinOrderValue,
            request.MaxUsages,
            request.ValidFrom,
            request.ValidTo
        );

        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>PUT /api/v1/backoffice/coupons/{id} — Atualiza cupom existente.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCoupon(Guid id, [FromBody] UpdateCouponRequest request, CancellationToken ct)
    {
        var command = new UpdateCouponCommand(
            id,
            request.Description,
            request.DiscountValue,
            request.MinOrderValue,
            request.MaxUsages,
            request.ValidFrom,
            request.ValidTo,
            IsActive: null
        );

        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>PATCH /api/v1/backoffice/coupons/{id}/activate — Ativa cupom.</summary>
    [HttpPatch("{id:guid}/activate")]
    public async Task<IActionResult> ActivateCoupon(Guid id, CancellationToken ct)
    {
        var command = new UpdateCouponCommand(id, null, null, null, null, null, null, IsActive: true);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>PATCH /api/v1/backoffice/coupons/{id}/deactivate — Desativa cupom.</summary>
    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateCoupon(Guid id, CancellationToken ct)
    {
        var command = new UpdateCouponCommand(id, null, null, null, null, null, null, IsActive: false);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>DELETE /api/v1/backoffice/coupons/{id} — Remove cupom.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCoupon(Guid id, CancellationToken ct)
    {
        var command = new DeleteCouponCommand(id);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>
    /// POST /api/v1/backoffice/coupons/validate — Valida cupom para um valor de pedido.
    /// Permite acesso anônimo para que o frontend valide durante o checkout.
    /// </summary>
    [HttpPost("validate")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponRequest request, CancellationToken ct)
    {
        var query = new GetCouponByCodeQuery(request.Code, request.OrderTotal);
        var result = await _mediator.Send(query, ct);
        return HandleResult(result);
    }
}
