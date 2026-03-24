namespace FSI.CloudShopping.Domain.Entities;

using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;

/// <summary>
/// Aggregate root for Coupon. Represents a discount coupon.
/// </summary>
public class Coupon : AggregateRoot<Guid>
{
    public string Code { get; private set; }
    public string? Description { get; private set; }
    public CouponDiscountType DiscountType { get; private set; }
    public decimal DiscountValue { get; private set; }
    public Money? MinOrderValue { get; private set; }
    public int MaxUsages { get; private set; }
    public int CurrentUsages { get; private set; }
    public DateTime ValidFrom { get; private set; }
    public DateTime ValidTo { get; private set; }
    public bool IsActive { get; private set; }

    public Coupon(Guid id, string code, string? description, CouponDiscountType discountType,
        decimal discountValue, Money? minOrderValue, int maxUsages, DateTime validFrom, DateTime validTo, bool isActive = true)
        : base(id)
    {
        Code = code;
        Description = description;
        DiscountType = discountType;
        DiscountValue = discountValue;
        MinOrderValue = minOrderValue;
        MaxUsages = maxUsages;
        CurrentUsages = 0;
        ValidFrom = validFrom;
        ValidTo = validTo;
        IsActive = isActive;
    }

    protected Coupon() { }

    public static Coupon Create(string code, string? description, CouponDiscountType discountType,
        decimal discountValue, Money? minOrderValue, int maxUsages, DateTime validFrom, DateTime validTo)
    {
        return new Coupon(Guid.NewGuid(), code, description, discountType, discountValue,
            minOrderValue, maxUsages, validFrom, validTo, true);
    }

    public bool IsValid()
    {
        if (!IsActive)
            return false;

        var now = DateTime.UtcNow;
        if (now < ValidFrom || now > ValidTo)
            return false;

        if (MaxUsages > 0 && CurrentUsages >= MaxUsages)
            return false;

        return true;
    }

    public bool IsValid(Money orderTotal)
    {
        if (!IsValid())
            return false;

        if (MinOrderValue != null && orderTotal.IsLessThan(MinOrderValue))
            return false;

        return true;
    }

    public Money Apply(Money orderTotal)
    {
        if (!IsValid(orderTotal))
            throw new DomainException("Coupon cannot be applied to this order.");

        return DiscountType == CouponDiscountType.Percentage
            ? new Money(orderTotal.Amount * (DiscountValue / 100))
            : new Money(DiscountValue);
    }

    public void IncrementUsage()
    {
        if (CurrentUsages >= MaxUsages && MaxUsages > 0)
            throw new DomainException("Coupon usage limit reached.");

        CurrentUsages++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DecrementUsage()
    {
        if (CurrentUsages > 0)
        {
            CurrentUsages--;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string? description, decimal discountValue, Money? minOrderValue,
        int maxUsages, DateTime validFrom, DateTime validTo)
    {
        Description = description;
        DiscountValue = discountValue;
        MinOrderValue = minOrderValue;
        MaxUsages = maxUsages;
        ValidFrom = validFrom;
        ValidTo = validTo;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum CouponDiscountType
{
    Percentage = 0,
    Fixed = 1
}
