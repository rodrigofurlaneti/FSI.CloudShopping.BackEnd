namespace FSI.CloudShopping.Application.Queries.Order;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record GetOrdersByCustomerQuery(Guid CustomerId, int Page = 1, int PageSize = 10) : IRequest<Result<PagedResult<OrderSummaryDto>>>;

public class GetOrdersByCustomerQueryHandler : IRequestHandler<GetOrdersByCustomerQuery, Result<PagedResult<OrderSummaryDto>>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrdersByCustomerQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<OrderSummaryDto>>> Handle(
        GetOrdersByCustomerQuery request, CancellationToken cancellationToken)
    {
        var (orders, totalCount) = await _orderRepository.GetByCustomerIdPagedAsync(
            request.CustomerId, request.Page, request.PageSize, cancellationToken);

        var summaryDtos = orders
            .Select(order => new OrderSummaryDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount.Amount,
                ItemCount = order.Items.Count
            })
            .ToList();

        var result = new PagedResult<OrderSummaryDto>(summaryDtos, totalCount, request.Page, request.PageSize);
        return new Result<PagedResult<OrderSummaryDto>>.Success(result);
    }
}
