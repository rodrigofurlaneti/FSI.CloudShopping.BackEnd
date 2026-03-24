namespace FSI.CloudShopping.Application.Commands.Order;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;

public record CancelOrderCommand(int OrderId, Guid CustomerId, string? Reason = null) : IRequest<Result<OrderDto>>;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return new Result<OrderDto>.Failure("Order not found");
        }

        if (order.CustomerId != request.CustomerId)
        {
            return new Result<OrderDto>.Failure("Order does not belong to this customer");
        }

        if (!order.CanBeCancelled)
        {
            return new Result<OrderDto>.Failure($"Order cannot be cancelled with status {order.Status}");
        }

        // Release reserved stock for each item
        foreach (var item in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product != null)
            {
                product.ReleaseReservedStock(item.Quantity);
            }
        }

        order.Cancel(request.Reason);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var orderDto = _mapper.Map<OrderDto>(order);
        return new Result<OrderDto>.Success(orderDto);
    }
}
