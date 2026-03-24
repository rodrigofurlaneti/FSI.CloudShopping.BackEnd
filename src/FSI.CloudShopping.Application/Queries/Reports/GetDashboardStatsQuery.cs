namespace FSI.CloudShopping.Application.Queries.Reports;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Enums;

public record GetDashboardStatsQuery() : IRequest<Result<DashboardStatsDto>>;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;

    public GetDashboardStatsQueryHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task<Result<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
    {
        var today = DateTime.UtcNow.Date;
        var weekAgo = today.AddDays(-7);
        var monthAgo = today.AddMonths(-1);

        // Get orders from today
        var (todayOrdersList, _) = await _orderRepository.GetOrdersPagedAsync(1, 100, null, today, today.AddDays(1), null, cancellationToken);
        var todayRevenue = todayOrdersList.Sum(o => o.TotalAmount.Amount);

        // Get orders from this week
        var (weekOrdersList, _) = await _orderRepository.GetOrdersPagedAsync(1, 100, null, weekAgo, today.AddDays(1), null, cancellationToken);
        var weekRevenue = weekOrdersList.Sum(o => o.TotalAmount.Amount);

        // Get orders from this month
        var (monthOrdersList, _) = await _orderRepository.GetOrdersPagedAsync(1, 100, null, monthAgo, today.AddDays(1), null, cancellationToken);
        var monthRevenue = monthOrdersList.Sum(o => o.TotalAmount.Amount);

        // Count orders by status
        var (allOrdersList, _) = await _orderRepository.GetOrdersPagedAsync(1, 1000, null, null, null, null, cancellationToken);
        var pendingCount = allOrdersList.Count(o => o.Status == OrderStatus.Pending);
        var confirmedCount = allOrdersList.Count(o => o.Status == OrderStatus.Confirmed);
        var processingCount = allOrdersList.Count(o => o.Status == OrderStatus.Processing);
        var shippedCount = allOrdersList.Count(o => o.Status == OrderStatus.Shipped);

        // Get top products by sales (simplified - just top 5 by price * quantity)
        var allProducts = await _productRepository.GetAllAsync(cancellationToken);
        var topProducts = allProducts
            .OrderByDescending(p => p.Price.Amount * (p.StockQuantity - p.ReservedQuantity))
            .Take(5)
            .Select(p => new TopProductDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                QuantitySold = p.StockQuantity - p.GetAvailableStock(),
                Revenue = p.Price.Amount * (p.StockQuantity - p.GetAvailableStock())
            })
            .ToList();

        // Get low stock products
        var lowStockProducts = allProducts.Count(p => p.StockQuantity <= p.MinStockAlert);

        // Get new customers today
        var allCustomers = await _customerRepository.GetAllAsync(cancellationToken);
        var newCustomersToday = allCustomers.Count(c => c.CreatedAt.Date == today);
        var newCustomersWeek = allCustomers.Count(c => c.CreatedAt > weekAgo);

        // Get recent orders
        var recentOrders = allOrdersList
            .OrderByDescending(o => o.OrderDate)
            .Take(10)
            .Select(o => new RecentOrderSummaryDto
            {
                OrderId = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerEmail = o.Customer?.Email.Value ?? string.Empty,
                TotalAmount = o.TotalAmount.Amount,
                Status = o.Status.ToString(),
                OrderDate = o.OrderDate
            })
            .ToList();

        var stats = new DashboardStatsDto
        {
            TodayRevenue = todayRevenue,
            WeekRevenue = weekRevenue,
            MonthRevenue = monthRevenue,
            PendingOrders = pendingCount,
            ConfirmedOrders = confirmedCount,
            ProcessingOrders = processingCount,
            ShippedOrders = shippedCount,
            LowStockProducts = lowStockProducts,
            TopProducts = topProducts,
            RecentOrders = recentOrders
        };

        return new Result<DashboardStatsDto>.Success(stats);
    }
}
