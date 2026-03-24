namespace FSI.CloudShopping.WebAPI.Controllers.BackOffice;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Queries.Customer;
using FSI.CloudShopping.WebAPI.Controllers;

[ApiController]
[Route("api/v1/backoffice/customers")]
[Authorize(Policy = "BackOfficePolicy")]
public class CustomersBackOfficeController : BaseApiController
{
    private readonly IMediator _mediator;

    public CustomersBackOfficeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>GET /api/v1/backoffice/customers — Lista paginada com filtros.</summary>
    [HttpGet]
    public async Task<IActionResult> GetCustomers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? customerType = null,
        [FromQuery] string? search = null,
        CancellationToken ct = default)
    {
        var query = new GetCustomersPagedQuery(page, pageSize, customerType, search);
        var result = await _mediator.Send(query, ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/backoffice/customers/{id} — Detalhe do cliente com histórico de pedidos.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomer(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetCustomerByIdQuery(id), ct);
        return HandleResult(result);
    }
}
