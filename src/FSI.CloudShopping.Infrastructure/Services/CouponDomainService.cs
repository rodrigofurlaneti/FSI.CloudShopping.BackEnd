namespace FSI.CloudShopping.Infrastructure.Services;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Services;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Serviço de domínio para validação e aplicação de cupons de desconto.
/// </summary>
public class CouponDomainService : ICouponDomainService
{
    private readonly ICouponRepository _couponRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CouponDomainService(ICouponRepository couponRepository, IUnitOfWork unitOfWork)
    {
        _couponRepository = couponRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool IsValid, Money Discount, string? ErrorMessage)> ValidateAndApplyAsync(
        string couponCode, Money orderTotal, CancellationToken cancellationToken = default)
    {
        var coupon = await _couponRepository.GetByCodeAsync(couponCode, cancellationToken);

        if (coupon == null)
            return (false, new Money(0), "Cupom não encontrado.");

        if (!coupon.IsValid(orderTotal))
            return (false, new Money(0), "Cupom inválido, expirado ou valor mínimo não atingido.");

        var discount = coupon.Apply(orderTotal);
        coupon.IncrementUsage();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return (true, discount, null);
    }
}
