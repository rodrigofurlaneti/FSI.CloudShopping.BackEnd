namespace FSI.CloudShopping.Application.Queries.Reports;

using MediatR;
using FSI.CloudShopping.Application.Common;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Domain.Interfaces;

public enum ReportGroupBy
{
    Daily = 0,
    Weekly = 1,
    Monthly = 2
}

public record GetSalesReportQuery(DateTime DateFrom, DateTime DateTo, ReportGroupBy GroupBy) : IRequest<Result<SalesReportDto>>;

public class GetSalesReportQueryHandler : IRequestHandler<GetSalesReportQuery, Result<SalesReportDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetSalesReportQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<SalesReportDto>> Handle(GetSalesReportQuery request, CancellationToken cancellationToken)
    {
        var (ordersList, _) = await _orderRepository.GetOrdersPagedAsync(
            1, 1000,
            null,
            request.DateFrom,
            request.DateTo,
            null,
            cancellationToken);

        var orders = ordersList.ToList();

        if (orders.Count == 0)
        {
            var emptyReport = new SalesReportDto
            {
                FromDate = request.DateFrom,
                ToDate = request.DateTo,
                TotalRevenue = 0,
                TotalOrders = 0,
                AverageOrderValue = 0,
                DailyRevenue = [],
                TopProducts = [],
                CategoryRevenue = []
            };
            return new Result<SalesReportDto>.Success(emptyReport);
        }

        var totalRevenue = orders.Sum(o => o.TotalAmount.Amount);
        var totalOrders = orders.Count;
        var averageOrderValue = totalRevenue / totalOrders;

        // Group by date/week/month
        var dailyRevenue = orders
            .GroupBy(o => request.GroupBy == ReportGroupBy.Daily
                ? o.OrderDate.Date
                : request.GroupBy == ReportGroupBy.Weekly
                    ? o.OrderDate.Date.AddDays(-(int)o.OrderDate.DayOfWeek)
                    : new DateTime(o.OrderDate.Year, o.OrderDate.Month, 1))
            .Select(g => new DailyRevenueDto
            {
                Date = g.Key,
                Revenue = g.Sum(o => o.TotalAmount.Amount),
                OrderCount = g.Count()
            })
            .OrderBy(d => d.Date)
            .ToList();

        // Top products
        var topProducts = orders
            .SelectMany(o => o.Items)
            .GroupBy(i => new { i.ProductId, i.ProductName, i.ProductSku })
            .Select(g => new TopProductSalesDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                Sku = g.Key.ProductSku,
                Quantity = g.Sum(i => i.Quantity),
                TotalRevenue = g.Sum(i => i.Quantity * i.UnitPrice.Amount)
            })
            .OrderByDescending(p => p.TotalRevenue)
            .Take(10)
            .ToList();

        // Category revenue (simplified - would need category info on order items)
        var categoryRevenue = new List<CategoryRevenueDto>();

        var report = new SalesReportDto
        {
            FromDate = request.DateFrom,
            ToDate = request.DateTo,
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            AverageOrderValue = averageOrderValue,
            DailyRevenue = dailyRevenue,
            TopProducts = topProducts,
            CategoryRevenue = categoryRevenue
        };

        return new Result<SalesReportDto>.Success(report);
    }
}
