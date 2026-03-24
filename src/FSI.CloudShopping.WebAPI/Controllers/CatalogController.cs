namespace FSI.CloudShopping.WebAPI.Controllers;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FSI.CloudShopping.Application.Queries.Product;

[ApiController]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class CatalogController : BaseApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>GET /api/v1/catalog/products - Listagem paginada com filtros.</summary>
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery] string? search = null,
        [FromQuery] Guid? categoryId = null,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetProductsPagedQuery(page, pageSize, search, categoryId, "Active"), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/catalog/products/featured - Produtos em destaque.</summary>
    [HttpGet("products/featured")]
    public async Task<IActionResult> GetFeaturedProducts(
        [FromQuery] int limit = 8, CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetProductsPagedQuery(1, limit, null, null, "Active"), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/catalog/products/{id} - Detalhe do produto por ID.</summary>
    [HttpGet("products/{id:int}")]
    public async Task<IActionResult> GetProductById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/catalog/categories/{categoryId}/products - Produtos de uma categoria.</summary>
    [HttpGet("categories/{categoryId:guid}/products")]
    public async Task<IActionResult> GetProductsByCategory(
        Guid categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetProductsPagedQuery(page, pageSize, null, categoryId, "Active"), ct);
        return HandleResult(result);
    }

    /// <summary>GET /api/v1/catalog/search?q= - Busca textual de produtos.</summary>
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { success = false, message = "O termo de busca e obrigatorio." });

        var result = await _mediator.Send(
            new GetProductsPagedQuery(page, pageSize, q, null, "Active"), ct);
        return HandleResult(result);
    }
}
