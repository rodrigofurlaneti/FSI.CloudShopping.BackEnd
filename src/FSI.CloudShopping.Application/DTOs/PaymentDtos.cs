namespace FSI.CloudShopping.Application.DTOs;

using FSI.CloudShopping.Domain.Enums;

public class PaymentDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Method { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public string? GatewayResponse { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public string? FailureReason { get; set; }
    public int RetryCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public record ProcessPaymentRequest(int OrderId, string PaymentMethod, decimal Amount, string? GatewayToken = null);

public record ProcessManualPaymentRequest(int OrderId, string PaymentMethod, decimal Amount, string? BankSlipUrl = null);

public record RefundPaymentRequest(int PaymentId, string? Reason = null);

public record RetryPaymentRequest(int PaymentId);

public record ManualPaymentApprovalRequest(int PaymentId, string? Notes = null);

public record ManualPaymentRejectionRequest(int PaymentId, string Reason);
