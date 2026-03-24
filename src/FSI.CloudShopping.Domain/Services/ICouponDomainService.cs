namespace FSI.CloudShopping.Domain.Services;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

public interface ICouponDomainService
{
    Task<(bool IsValid, Money Discount, string? ErrorMessage)> ValidateAndApplyAsync(
        string couponCode, Money orderTotal, CancellationToken cancellationToken = default);
}
