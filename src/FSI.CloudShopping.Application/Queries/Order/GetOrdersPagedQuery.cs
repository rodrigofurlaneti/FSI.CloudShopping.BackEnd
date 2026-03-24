namespace FSI.CloudShopping.Application.Queries.Order;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;

public record GetOrdersPagedQuery(
    int Page = 1,
    int PageSize = 10,
    OrderStatus? Status = null,
    DateTime? DateFrom = null,
    DateTime? DateTo = null,
    string? SearchTerm = null
) : IRequest<Result<PagedResult<OrderDto>>>;

public class GetOrdersPagedQueryHandler : IRequestHandler<GetOrdersPagedQuery, Result<PagedResult<OrderDto>>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersPagedQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<OrderDto>>> Handle(
        GetOrdersPagedQuery request, CancellationToken cancellationToken)
    {
        var (orders, totalCount) = await _orderRepository.GetOrdersPagedAsync(
            request.Page,
            request.PageSize,
            request.Status,
            request.DateFrom,
            request.DateTo,
            request.SearchTerm,
            cancellationToken);

        var orderDtos = orders
            .Select(order => _mapper.Map<OrderDto>(order))
            .ToList();

        var result = new PagedResult<OrderDto>(orderDtos, totalCount, request.Page, request.PageSize);
        return new Result<PagedResult<OrderDto>>.Success(result);
    }
}
