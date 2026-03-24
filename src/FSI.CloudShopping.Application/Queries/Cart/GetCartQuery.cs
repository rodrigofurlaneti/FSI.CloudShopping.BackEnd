namespace FSI.CloudShopping.Application.Queries.Cart;

using MediatR;
using AutoMapper;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public record GetCartQuery(Guid SessionToken) : IRequest<Result<CartDto>>;

public class GetCartQueryHandler : IRequestHandler<GetCartQuery, Result<CartDto>>
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public GetCartQueryHandler(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetBySessionTokenAsync(request.SessionToken, cancellationToken);

        if (cart == null)
        {
            // Return empty cart DTO instead of failure
            var emptyCartDto = new CartDto
            {
                Id = 0,
                CustomerId = Guid.Empty,
                Items = [],
                Total = 0,
                ItemCount = 0,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                IsExpired = false
            };
            return new Result<CartDto>.Success(emptyCartDto);
        }

        var cartDto = _mapper.Map<CartDto>(cart);
        return new Result<CartDto>.Success(cartDto);
    }
}
