namespace FSI.CloudShopping.Application.Queries.Order;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record GetOrderByNumberQuery(string OrderNumber, Guid? CustomerId = null) : IRequest<Result<OrderDto>>;

public class GetOrderByNumberQueryHandler : IRequestHandler<GetOrderByNumberQuery, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderByNumberQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(GetOrderByNumberQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(request.OrderNumber, cancellationToken);

        if (order == null)
        {
            return new Result<OrderDto>.Failure("Order not found");
        }

        // If CustomerId is provided, verify ownership
        if (request.CustomerId.HasValue && order.CustomerId != request.CustomerId.Value)
        {
            return new Result<OrderDto>.Failure("Order does not belong to this customer");
        }

        var orderDto = _mapper.Map<OrderDto>(order);
        return new Result<OrderDto>.Success(orderDto);
    }
}
