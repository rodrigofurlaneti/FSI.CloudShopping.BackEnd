namespace FSI.CloudShopping.Application.Commands.Cart;

using MediatR;
using FSI.CloudShopping.Domain.Core;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record AddToCartCommand(
    Guid SessionToken,
    int ProductId,
    string ProductName,
    string ProductImageUrl,
    string ProductSku,
    int Quantity,
    decimal UnitPrice
) : IRequest<Result<CartDto>>;

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddToCartCommandHandler(
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

    public async Task<Result<CartDto>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        // Get or create cart
        var cart = await _cartRepository.GetBySessionTokenAsync(request.SessionToken, cancellationToken);
        if (cart == null)
        {
            return new Result<CartDto>.Failure("Cart not found for this session");
        }

        // Validate product exists
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
        {
            return new Result<CartDto>.Failure("Product not found");
        }

        // Validate product has enough stock
        if (product.GetAvailableStock() < request.Quantity)
        {
            return new Result<CartDto>.Failure($"Insufficient stock. Available: {product.GetAvailableStock()}, Requested: {request.Quantity}");
        }

        // Add or update item in cart
        var quantity = new Quantity(request.Quantity);
        var unitPrice = new Money(request.UnitPrice);
        cart.AddOrUpdateItem(request.ProductId, request.ProductName, request.ProductImageUrl, request.ProductSku, quantity, unitPrice);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var cartDto = _mapper.Map<CartDto>(cart);
        return new Result<CartDto>.Success(cartDto);
    }
}
