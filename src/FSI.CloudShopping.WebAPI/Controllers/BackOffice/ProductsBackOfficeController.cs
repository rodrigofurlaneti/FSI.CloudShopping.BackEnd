namespace FSI.CloudShopping.WebAPI.Controllers.BackOffice;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Commands.Product;
using FSI.CloudShopping.Application.Queries.Product;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.WebAPI.Controllers;

[ApiController]
[Route("api/v1/backoffice/products")]
[Authorize(Policy = "BackOfficePolicy")]
public class ProductsBackOfficeController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProductsBackOfficeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>GET /api/v1/backoffice/products — Lista paginada com filtros.</summary>
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] string? status = null,
        CancellationToken ct = default)
    {
        var query = new GetProductsPagedQuery(page, pageSize, search, categoryId, status);
        var result = await _mediator.Send(query, ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/backoffice/products/{id} — Detalhe do produto.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProduct(int id, CancellationToken ct)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query, ct);
        return HandleResult(result);
    }

    /// <summary>POST /api/v1/backoffice/products — Cria novo produto.</summary>
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request, CancellationToken ct)
    {
        var command = new CreateProductCommand(
            request.Sku, request.Name, request.Description, request.ShortDescription,
            request.Price, request.CompareAtPrice, request.CostPrice,
            request.StockQuantity, request.MinStockAlert, request.CategoryId,
            request.ImageUrl, request.Weight, request.IsFeatured);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>PUT /api/v1/backoffice/products/{id} — Atualiza produto.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request, CancellationToken ct)
    {
        var command = new UpdateProductCommand(
            id, request.Name, request.Description, request.ShortDescription,
            request.Price, request.CompareAtPrice, request.CostPrice,
            request.MinStockAlert, request.CategoryId,
            request.ImageUrl, request.Weight, request.IsFeatured, request.Status);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>PUT /api/v1/backoffice/products/{id}/stock — Atualiza estoque.</summary>
    [HttpPut("{id:int}/stock")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequest request, CancellationToken ct)
    {
        var command = new UpdateStockCommand(id, request.Quantity, request.Reason, request.IsAddition);
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }

    /// <summary>DELETE /api/v1/backoffice/products/{id} — Desativa produto (soft delete).</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken ct)
    {
        var command = new UpdateProductCommand(id, Status: "Discontinued");
        var result = await _mediator.Send(command, ct);
        return HandleResult(result);
    }
}
