namespace FSI.CloudShopping.Application.Commands.Order;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;

public record UpdateOrderStatusCommand(
    int OrderId,
    OrderStatus NewStatus,
    string? TrackingNumber = null,
    string? Notes = null
) : IRequest<Result<OrderDto>>;

public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderStatusCommandHandler(
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
        if (order == null)
        {
            return new Result<OrderDto>.Failure("Order not found");
        }

        try
        {
            switch (request.NewStatus)
            {
                case OrderStatus.Confirmed:
                    order.Confirm();
                    break;

                case OrderStatus.Processing:
                    order.StartProcessing();
                    break;

                case OrderStatus.Shipped:
                    if (string.IsNullOrEmpty(request.TrackingNumber))
                    {
                        return new Result<OrderDto>.Failure("Tracking number is required for shipping");
                    }
                    order.Ship(request.TrackingNumber);
                    break;

                case OrderStatus.Delivered:
                    order.Deliver();
                    break;

                case OrderStatus.Cancelled:
                    order.Cancel(request.Notes);
                    break;

                case OrderStatus.Refunded:
                    order.Refund();
                    break;

                default:
                    return new Result<OrderDto>.Failure($"Invalid order status: {request.NewStatus}");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var orderDto = _mapper.Map<OrderDto>(order);
            return new Result<OrderDto>.Success(orderDto);
        }
        catch (Domain.Core.DomainException ex)
        {
            return new Result<OrderDto>.Failure(ex.Message);
        }
    }
}
