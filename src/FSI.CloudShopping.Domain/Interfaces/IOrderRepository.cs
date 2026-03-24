namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Enums;

public interface IOrderRepository : IRepository<Order, int>
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Order> Orders, int TotalCount)> GetByCustomerIdPagedAsync(Guid customerId, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Order>> GetWithPaymentsAsync(int orderId, CancellationToken cancellationToken = default);
    Task<(IEnumerable<Order>, int)> GetPagedAsync(int pageNumber, int pageSize, OrderStatus? status = null, CancellationToken cancellationToken = default);

    Task<(IEnumerable<Order> Orders, int TotalCount)> GetOrdersPagedAsync(
        int page,
        int pageSize,
        OrderStatus? status = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default);
}
