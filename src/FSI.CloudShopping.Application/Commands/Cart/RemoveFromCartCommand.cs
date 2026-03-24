namespace FSI.CloudShopping.Application.Commands.Cart;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record RemoveFromCartCommand(Guid SessionToken, int ProductId) : IRequest<Result<CartDto>>;

public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RemoveFromCartCommandHandler(
        ICartRepository cartRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetBySessionTokenAsync(request.SessionToken, cancellationToken);
        if (cart == null)
        {
            return new Result<CartDto>.Failure("Cart not found for this session");
        }

        cart.RemoveItem(request.ProductId);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var cartDto = _mapper.Map<CartDto>(cart);
        return new Result<CartDto>.Success(cartDto);
    }
}
