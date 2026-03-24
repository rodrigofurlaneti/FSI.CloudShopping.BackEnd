namespace FSI.CloudShopping.WebAPI.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Commands.Customer;
using FSI.CloudShopping.Application.Queries.Customer;
using FSI.CloudShopping.Application.Queries.Order;
using FSI.CloudShopping.Application.DTOs;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CustomerController : BaseApiController
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>GET /api/v1/customers/me — Retorna perfil do cliente autenticado.</summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var result = await _mediator.Send(new GetCustomerByIdQuery(customerId), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/customers/me/addresses — Lista endereços do cliente.</summary>
    [HttpGet("me/addresses")]
    public async Task<IActionResult> GetAddresses(CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var result = await _mediator.Send(new GetCustomerByIdQuery(customerId), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/customers/me/addresses — Adiciona endereço.</summary>
    [HttpPost("me/addresses")]
    public async Task<IActionResult> AddAddress([FromBody] CreateAddressRequest request, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var command = new AddAddressCommand(
            customerId, request.AddressType, request.Street, request.Number,
            request.Complement, request.Neighborhood, request.City,
            request.State, request.ZipCode, request.IsDefault);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/customers/me/orders — Histórico de pedidos paginado.</summary>
    [HttpGet("me/orders")]
    public async Task<IActionResult> GetMyOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var customerId = GetCurrentCustomerId();
        var result = await _mediator.Send(new GetOrdersByCustomerQuery(customerId, page, pageSize), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/customers/me/orders/{orderNumber} — Detalhe de um pedido.</summary>
    [HttpGet("me/orders/{orderNumber}")]
    public async Task<IActionResult> GetMyOrder(string orderNumber, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var result = await _mediator.Send(new GetOrderByNumberQuery(orderNumber, customerId), ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/customers/upgrade/individual — Upgrade para B2C (PF).</summary>
    [HttpPost("upgrade/individual")]
    public async Task<IActionResult> UpgradeToIndividual([FromBody] UpgradeIndividualRequest request, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var command = new RegisterIndividualCommand(customerId, request.TaxId, request.FullName, request.BirthDate);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/customers/upgrade/company — Upgrade para B2B (PJ).</summary>
    [HttpPost("upgrade/company")]
    public async Task<IActionResult> UpgradeToCompany([FromBody] UpgradeCompanyRequest request, CancellationToken ct)
    {
        var customerId = GetCurrentCustomerId();
        var command = new RegisterCompanyCommand(customerId, request.BusinessTaxId, request.CompanyName, request.TradeName, request.StateTaxId);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }
}
