namespace FSI.CloudShopping.Application.Commands.Cart;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record UpdateCartItemCommand(Guid SessionToken, int ProductId, int Quantity) : IRequest<Result<CartDto>>;

public class UpdateCartItemCommandHandler : IRequestHandler<UpdateCartItemCommand, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCartItemCommandHandler(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetBySessionTokenAsync(request.SessionToken, cancellationToken);
        if (cart == null)
        {
            return new Result<CartDto>.Failure("Cart not found for this session");
        }

        // If quantity is 0, remove the item
        if (request.Quantity == 0)
        {
            cart.RemoveItem(request.ProductId);
        }
        else
        {
            // Validate product exists and has stock
            var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null)
            {
                return new Result<CartDto>.Failure("Product not found");
            }

            if (product.GetAvailableStock() < request.Quantity)
            {
                return new Result<CartDto>.Failure($"Insufficient stock. Available: {product.GetAvailableStock()}, Requested: {request.Quantity}");
            }

            // Find the item in cart and update quantity
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (cartItem == null)
            {
                return new Result<CartDto>.Failure("Item not found in cart");
            }

            var quantity = new FSI.CloudShopping.Domain.ValueObjects.Quantity(request.Quantity);
            cartItem.UpdateQuantity(quantity);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var cartDto = _mapper.Map<CartDto>(cart);
        return new Result<CartDto>.Success(cartDto);
    }
}
