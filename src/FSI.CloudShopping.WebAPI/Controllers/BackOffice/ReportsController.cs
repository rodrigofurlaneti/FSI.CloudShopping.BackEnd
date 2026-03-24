namespace FSI.CloudShopping.WebAPI.Controllers.BackOffice;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Queries.Reports;
using FSI.CloudShopping.WebAPI.Controllers;

[ApiController]
[Route("api/v1/backoffice/reports")]
[Authorize(Policy = "BackOfficePolicy")]
public class ReportsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>GET /api/v1/backoffice/reports/dashboard — KPIs do dia/semana/mês.</summary>
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDashboardStatsQuery(), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/backoffice/reports/sales — Relatório de vendas por período.</summary>
    [HttpGet("sales")]
    public async Task<IActionResult> GetSalesReport(
        [FromQuery] DateTime dateFrom,
        [FromQuery] DateTime dateTo,
        [FromQuery] string groupBy = "Daily",
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetSalesReportQuery(dateFrom, dateTo, groupBy), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/backoffice/reports/customers — Relatório de clientes.</summary>
    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomerReport(
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetSalesReportQuery(dateFrom ?? DateTime.UtcNow.AddMonths(-1), dateTo ?? DateTime.UtcNow, "Monthly"), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/backoffice/reports/inventory — Relatório de estoque.</summary>
    [HttpGet("inventory")]
    public async Task<IActionResult> GetInventoryReport(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetDashboardStatsQuery(), ct);
        return HandleResult(result);
    }
}
