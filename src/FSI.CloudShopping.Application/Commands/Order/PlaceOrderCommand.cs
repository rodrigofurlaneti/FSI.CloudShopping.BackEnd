namespace FSI.CloudShopping.Application.Commands.Order;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.ValueObjects;

public record PlaceOrderCommand(
    Guid CustomerId,
    Guid ShippingAddressId,
    PaymentMethod PaymentMethod,
    string? CouponCode = null
) : IRequest<Result<OrderDto>>;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<OrderDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PlaceOrderCommandHandler(
        ICartRepository cartRepository,
        IOrderRepository orderRepository,
        ICouponRepository couponRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
        _couponRepository = couponRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<OrderDto>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        // Get customer cart
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        if (cart == null || cart.Items.Count == 0)
        {
            return new Result<OrderDto>.Failure("Cart is empty or not found");
        }

        // Get customer to verify address
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
        {
            return new Result<OrderDto>.Failure("Customer not found");
        }

        var address = customer.Addresses.FirstOrDefault(a => a.Id == request.ShippingAddressId);
        if (address == null)
        {
            return new Result<OrderDto>.Failure("Shipping address not found");
        }

        // Calculate totals
        var subTotal = cart.GetTotal();
        var discountAmount = new Money(0);

        // Apply coupon if provided
        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            var coupon = await _couponRepository.GetByCodeAsync(request.CouponCode, cancellationToken);
            if (coupon == null || !coupon.IsValid(subTotal))
            {
                return new Result<OrderDto>.Failure("Coupon is invalid or expired");
            }

            discountAmount = coupon.Apply(subTotal);
            coupon.IncrementUsage();
        }

        // For now, shipping cost is 0 (can be enhanced with shipping service)
        var shippingCost = new Money(0);

        // Create order
        var order = Domain.Entities.Order.Create(
            request.CustomerId,
            request.ShippingAddressId,
            subTotal,
            discountAmount,
            shippingCost,
            request.CouponCode
        );

        // Add items from cart to order
        foreach (var item in cart.Items)
        {
            order.AddItem(item.ProductId, item.ProductName, item.ProductSku, item.Quantity, item.UnitPrice, new Money(0));
        }

        // Save order
        await _orderRepository.AddAsync(order, cancellationToken);

        // Clear cart
        cart.Clear();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var orderDto = _mapper.Map<OrderDto>(order);
        return new Result<OrderDto>.Success(orderDto);
    }
}
