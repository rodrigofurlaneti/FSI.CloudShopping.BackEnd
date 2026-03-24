namespace FSI.CloudShopping.Application.Interfaces;

public interface IPaymentGatewayService
{
    Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentInput input, CancellationToken cancellationToken = default);
    Task<RefundPaymentResult> RefundPaymentAsync(string transactionId, decimal amount, CancellationToken cancellationToken = default);
    Task<TransactionStatusResult> GetTransactionStatusAsync(string transactionId, CancellationToken cancellationToken = default);
}

public class ProcessPaymentInput
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string CardToken { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = [];
}

public class ProcessPaymentResult
{
    public bool IsSuccessful { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string? Message { get; set; }
    public string? AuthorizationCode { get; set; }
    public DateTime ProcessedAt { get; set; }
}

public class RefundPaymentResult
{
    public bool IsSuccessful { get; set; }
    public string RefundTransactionId { get; set; } = string.Empty;
    public string? Message { get; set; }
}

public class TransactionStatusResult
{
    public string TransactionId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}
