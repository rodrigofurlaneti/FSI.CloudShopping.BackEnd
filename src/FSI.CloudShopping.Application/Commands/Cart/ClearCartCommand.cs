namespace FSI.CloudShopping.Application.Commands.Cart;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Domain.Interfaces;

public record ClearCartCommand(Guid SessionToken) : IRequest<Result<bool>>;

public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<bool>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClearCartCommandHandler(ICartRepository cartRepository, IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetBySessionTokenAsync(request.SessionToken, cancellationToken);
        if (cart == null)
        {
            return new Result<bool>.Failure("Cart not found for this session");
        }

        cart.Clear();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new Result<bool>.Success(true);
    }
}
