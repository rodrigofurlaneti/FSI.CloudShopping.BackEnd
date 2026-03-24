namespace FSI.CloudShopping.Application.DTOs;

public class DashboardStatsDto
{
    public decimal TodayRevenue { get; set; }
    public decimal WeekRevenue { get; set; }
    public decimal MonthRevenue { get; set; }
    public int PendingOrders { get; set; }
    public int ConfirmedOrders { get; set; }
    public int ProcessingOrders { get; set; }
    public int ShippedOrders { get; set; }
    public int LowStockProducts { get; set; }
    public List<TopProductDto> TopProducts { get; set; } = [];
    public List<RecentOrderSummaryDto> RecentOrders { get; set; } = [];
}

public class SalesReportDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public List<DailyRevenueDto> DailyRevenue { get; set; } = [];
    public List<TopProductSalesDto> TopProducts { get; set; } = [];
    public List<CategoryRevenueDto> CategoryRevenue { get; set; } = [];
}

public class DailyRevenueDto
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
}

public class TopProductSalesDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class CategoryRevenueDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
}

public class TopProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public class RecentOrderSummaryDto
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
}

public class CustomerReportDto
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalCustomers { get; set; }
    public int NewCustomers { get; set; }
    public int ActiveCustomers { get; set; }
    public List<ConversionMetricDto> ConversionMetrics { get; set; } = [];
    public List<TopCustomerDto> TopCustomers { get; set; } = [];
}

public class ConversionMetricDto
{
    public string Period { get; set; } = string.Empty;
    public int GuestCount { get; set; }
    public int LeadCount { get; set; }
    public int B2CCount { get; set; }
    public int B2BCount { get; set; }
}

public class TopCustomerDto
{
    public Guid CustomerId { get; set; }
    public string Email { get; set; } = string.Empty;
    public int OrderCount { get; set; }
    public decimal TotalSpent { get; set; }
}

public class InventoryReportDto
{
    public int TotalProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int OutOfStockProducts { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public List<LowStockProductDto> LowStockItems { get; set; } = [];
}

public class LowStockProductDto
{
    public int ProductId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int MinStockAlert { get; set; }
    public decimal CostPrice { get; set; }
}

public class AuditLogDto
{
    public Guid Id { get; set; }
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public Guid? UserId { get; set; }
    public string? UserEmail { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProductManagementDto
{
    public int Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableStock { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
