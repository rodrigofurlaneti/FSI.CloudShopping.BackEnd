namespace FSI.CloudShopping.Application.Commands.Cart;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record MergeCartsCommand(Guid GuestToken, Guid CustomerId) : IRequest<Result<CartDto>>;

public class MergeCartsCommandHandler : IRequestHandler<MergeCartsCommand, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MergeCartsCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(MergeCartsCommand request, CancellationToken cancellationToken)
    {
        // Get guest cart
        var guestCart = await _cartRepository.GetBySessionTokenAsync(request.GuestToken, cancellationToken);
        if (guestCart == null)
        {
            return new Result<CartDto>.Failure("Guest cart not found");
        }

        // Get or create customer cart
        var customerCart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
        if (customerCart == null)
        {
            return new Result<CartDto>.Failure("Customer cart not found");
        }

        // Merge guest cart into customer cart
        customerCart.Merge(guestCart);

        // Delete guest cart
        await _cartRepository.DeleteAsync(guestCart, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var cartDto = _mapper.Map<CartDto>(customerCart);
        return new Result<CartDto>.Success(cartDto);
    }
}
