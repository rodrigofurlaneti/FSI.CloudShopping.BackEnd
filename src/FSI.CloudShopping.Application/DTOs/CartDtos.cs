namespace FSI.CloudShopping.Application.DTOs;

public class CartDto
{
    public int Id { get; set; }
    public Guid CustomerId { get; set; }
    public List<CartItemDto> Items { get; set; } = [];
    public decimal Total { get; set; }
    public int ItemCount { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsExpired { get; set; }
}

public class CartItemDto
{
    public Guid Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductImageUrl { get; set; } = string.Empty;
    public string ProductSku { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal { get; set; }
}

public record AddToCartRequest(int ProductId, string ProductName, string ProductImageUrl, string ProductSku, int Quantity, decimal UnitPrice);

public record UpdateCartItemRequest(int Quantity);

public record MergeCartsRequest(int OtherCartId);
