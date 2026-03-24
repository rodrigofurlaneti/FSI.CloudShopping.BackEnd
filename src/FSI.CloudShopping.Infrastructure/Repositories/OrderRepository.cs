namespace FSI.CloudShopping.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Infrastructure.Data;

public class OrderRepository : Repository<Order, int>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(o => o.Status == status)
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetWithPaymentsAsync(int orderId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(o => o.Id == orderId)
            .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Order>, int)> GetPagedAsync(int pageNumber, int pageSize, OrderStatus? status = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.Include(o => o.Items).AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (orders, totalCount);
    }

    public async Task<(IEnumerable<Order> Orders, int TotalCount)> GetByCustomerIdPagedAsync(
        Guid customerId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.OrderDate);

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (orders, totalCount);
    }

    public async Task<(IEnumerable<Order> Orders, int TotalCount)> GetOrdersPagedAsync(
        int page,
        int pageSize,
        OrderStatus? status = null,
        DateTime? dateFrom = null,
        DateTime? dateTo = null,
        string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet
            .Include(o => o.Items)
            .Include(o => o.Customer)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(o => o.Status == status.Value);

        if (dateFrom.HasValue)
            query = query.Where(o => o.OrderDate >= dateFrom.Value);

        if (dateTo.HasValue)
            query = query.Where(o => o.OrderDate <= dateTo.Value);

        if (!string.IsNullOrEmpty(searchTerm))
            query = query.Where(o => o.OrderNumber.Contains(searchTerm)
                || o.Customer!.Email.Value.Contains(searchTerm));

        var totalCount = await query.CountAsync(cancellationToken);

        var orders = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (orders, totalCount);
    }
}
