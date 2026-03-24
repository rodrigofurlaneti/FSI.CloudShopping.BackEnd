namespace FSI.CloudShopping.Application.Queries.Product;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public class GetProductsPagedQueryHandler : IRequestHandler<GetProductsPagedQuery, Result<PagedResult<ProductSummaryDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductsPagedQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<ProductSummaryDto>>> Handle(
        GetProductsPagedQuery request, CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            request.Search,
            request.CategoryId,
            request.Status,
            cancellationToken);

        var productDtos = _mapper.Map<IEnumerable<ProductSummaryDto>>(products);
        var pagedResult = new PagedResult<ProductSummaryDto>(
            productDtos, totalCount, request.PageNumber, request.PageSize);

        return new Result<PagedResult<ProductSummaryDto>>.Success(pagedResult);
    }
}
