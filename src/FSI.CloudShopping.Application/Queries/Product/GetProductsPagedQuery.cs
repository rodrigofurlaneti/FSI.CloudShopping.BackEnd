namespace FSI.CloudShopping.Application.Queries.Product;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;

public record GetProductsPagedQuery(
    int PageNumber,
    int PageSize,
    string? Search = null,
    Guid? CategoryId = null,
    string? Status = null
) : IRequest<Result<PagedResult<ProductSummaryDto>>>;
