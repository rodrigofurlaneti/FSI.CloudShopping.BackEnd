namespace FSI.CloudShopping.Infrastructure.Services;

using System.Net.Http.Json;
using System.Text.Json;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

/// <summary>
/// MercadoPago payment gateway integration.
/// Implements IPaymentGatewayService following Clean Architecture (Infrastructure layer).
/// Supports: Pix, Credit/Debit Card, Bank Slip, Manual Transfer.
/// </summary>
public sealed class MercadoPagoPaymentService : IPaymentGatewayService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MercadoPagoPaymentService> _logger;
    private const string BaseUrl = "https://api.mercadopago.com";

    public MercadoPagoPaymentService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<MercadoPagoPaymentService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        var accessToken = configuration["PaymentGateway:MercadoPago:AccessToken"] ?? string.Empty;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
    }

    // ── IPaymentGatewayService ────────────────────────────────────────────────

    public async Task<ProcessPaymentResult> ProcessPaymentAsync(
        ProcessPaymentInput input, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = new
            {
                transaction_amount = input.Amount,
                description = input.Description ?? $"Order {input.OrderId}",
                token = input.CardToken,
                external_reference = input.OrderId.ToString()
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/v1/payments", payload, cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("MercadoPago error: {Status} - {Content}", response.StatusCode, content);
                return new ProcessPaymentResult
                {
                    IsSuccessful = false,
                    Message = $"Gateway error: {response.StatusCode}",
                    ProcessedAt = DateTime.UtcNow
                };
            }

            var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            var status = root.GetProperty("status").GetString();
            var id = root.GetProperty("id").GetInt64().ToString();

            return new ProcessPaymentResult
            {
                IsSuccessful = status is "approved",
                TransactionId = id,
                Message = status,
                AuthorizationCode = id,
                ProcessedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment via MercadoPago.");
            return new ProcessPaymentResult { IsSuccessful = false, Message = ex.Message, ProcessedAt = DateTime.UtcNow };
        }
    }

    public async Task<RefundPaymentResult> RefundPaymentAsync(
        string transactionId, decimal amount, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/v1/payments/{transactionId}/refunds",
                new { amount },
                cancellationToken);

            return new RefundPaymentResult
            {
                IsSuccessful = response.IsSuccessStatusCode,
                RefundTransactionId = transactionId,
                Message = response.IsSuccessStatusCode ? "Refunded" : "Refund failed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refunding payment {TransactionId}", transactionId);
            return new RefundPaymentResult { IsSuccessful = false, Message = ex.Message };
        }
    }

    public async Task<TransactionStatusResult> GetTransactionStatusAsync(
        string transactionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{BaseUrl}/v1/payments/{transactionId}", cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
                return new TransactionStatusResult { TransactionId = transactionId, Status = "error" };

            var doc = JsonDocument.Parse(content);
            return new TransactionStatusResult
            {
                TransactionId = transactionId,
                Status = doc.RootElement.GetProperty("status").GetString() ?? "unknown",
                Amount = doc.RootElement.TryGetProperty("transaction_amount", out var amtProp)
                    ? amtProp.GetDecimal() : 0,
                CreatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transaction status {TransactionId}", transactionId);
            return new TransactionStatusResult { TransactionId = transactionId, Status = "error" };
        }
    }

    /// <summary>
    /// Charges a Payment domain aggregate. Used by the SAGA ProcessPaymentStep.
    /// Maps the Payment entity to a gateway request and returns a structured result.
    /// </summary>
    public async Task<GatewayChargeResult> ChargeAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        try
        {
            var paymentMethodId = payment.Method switch
            {
                PaymentMethod.Pix => "pix",
                PaymentMethod.BankSlip => "bolbradesco",
                PaymentMethod.CreditCard => "credit_card",
                PaymentMethod.DebitCard => "debit_card",
                PaymentMethod.ManualTransfer => "bank_transfer",
                _ => "pix"
            };

            var payload = new
            {
                transaction_amount = payment.Amount.Amount,
                payment_method_id = paymentMethodId,
                external_reference = payment.OrderId.ToString()
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/v1/payments", payload, cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                return GatewayChargeResult.Fail($"Gateway rejected: {response.StatusCode}");

            var doc = JsonDocument.Parse(content);
            var status = doc.RootElement.GetProperty("status").GetString();
            var txnId = doc.RootElement.GetProperty("id").GetInt64().ToString();

            return status is "approved" or "pending"
                ? GatewayChargeResult.Ok(txnId, content)
                : GatewayChargeResult.Fail($"Gateway status: {status}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error charging payment {PaymentId} via MercadoPago.", payment.Id);
            return GatewayChargeResult.Fail(ex.Message);
        }
    }
}
