namespace FSI.CloudShopping.Application.Queries.Coupon;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;

public record GetCouponByCodeQuery(string Code, decimal OrderTotal = 0) : IRequest<Result<ValidateCouponResponse>>;

public class GetCouponByCodeQueryHandler : IRequestHandler<GetCouponByCodeQuery, Result<ValidateCouponResponse>>
{
    private readonly ICouponRepository _couponRepository;

    public GetCouponByCodeQueryHandler(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<Result<ValidateCouponResponse>> Handle(
        GetCouponByCodeQuery request, CancellationToken cancellationToken)
    {
        var coupon = await _couponRepository.GetByCodeAsync(request.Code, cancellationToken);

        if (coupon == null || !coupon.IsActive)
            return new Result<ValidateCouponResponse>.Success(
                new ValidateCouponResponse(false, 0, "Cupom não encontrado ou inativo."));

        var orderTotal = new Money(request.OrderTotal);

        if (!coupon.IsValid(orderTotal))
            return new Result<ValidateCouponResponse>.Success(
                new ValidateCouponResponse(false, 0, "Cupom inválido, expirado ou valor mínimo não atingido."));

        var discount = coupon.Apply(orderTotal);
        return new Result<ValidateCouponResponse>.Success(
            new ValidateCouponResponse(true, discount.Amount, $"Desconto de R$ {discount.Amount:F2} aplicado."));
    }
}
