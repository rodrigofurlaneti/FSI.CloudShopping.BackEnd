namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;

public interface ICouponRepository : IRepository<Coupon, Guid>
{
    Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Coupon>, int)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Coupon>> GetActiveAsync(CancellationToken cancellationToken = default);
}
