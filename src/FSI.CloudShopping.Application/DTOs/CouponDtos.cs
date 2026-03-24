namespace FSI.CloudShopping.Application.DTOs;

public class CouponDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DiscountType { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
    public decimal? MinOrderValue { get; set; }
    public int MaxUsages { get; set; }
    public int CurrentUsages { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public bool IsActive { get; set; }
}

public record CreateCouponRequest(string Code, string? Description, string DiscountType, decimal DiscountValue, decimal? MinOrderValue, int MaxUsages, DateTime ValidFrom, DateTime ValidTo);

public record UpdateCouponRequest(string? Description = null, decimal? DiscountValue = null, decimal? MinOrderValue = null, int? MaxUsages = null, DateTime? ValidFrom = null, DateTime? ValidTo = null);

public record ValidateCouponRequest(string Code, decimal OrderTotal);

public record ValidateCouponResponse(bool IsValid, decimal DiscountAmount, string? Message = null);
