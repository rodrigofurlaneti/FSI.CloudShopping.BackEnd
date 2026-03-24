namespace FSI.CloudShopping.Infrastructure.Services;

using System.Net.Http.Json;
using System.Text.Json;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Enums;
using FSI.CloudShopping.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

/// <summary>
/// Serviço de integração com MercadoPago.
/// Suporta Pix, Cartão de Crédito/Débito e Boleto Bancário.
/// </summary>
public class MercadoPagoPaymentService : IPaymentGatewayService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MercadoPagoPaymentService> _logger;
    private readonly string _accessToken;
    private const string BaseUrl = "https://api.mercadopago.com";

    public MercadoPagoPaymentService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<MercadoPagoPaymentService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _accessToken = configuration["PaymentGateway:MercadoPago:AccessToken"] ?? string.Empty;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
    }

    public async Task<PaymentGatewayResult> ProcessPaymentAsync(
        PaymentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = BuildPaymentPayload(request);
            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/v1/payments", payload, cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("MercadoPago error: {Status} - {Content}",
                    response.StatusCode, content);
                return PaymentGatewayResult.Failed($"Gateway error: {response.StatusCode}");
            }

            var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;
            var status = root.GetProperty("status").GetString();
            var id = root.GetProperty("id").GetInt64().ToString();

            return status switch
            {
                "approved" => PaymentGatewayResult.Approved(id, content),
                "pending" => PaymentGatewayResult.Pending(id, content),
                "in_process" => PaymentGatewayResult.Pending(id, content),
                _ => PaymentGatewayResult.Failed($"Payment status: {status}")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment through MercadoPago");
            return PaymentGatewayResult.Failed("Unexpected error during payment processing.");
        }
    }

    public async Task<PaymentGatewayResult> RefundPaymentAsync(
        string transactionId, decimal amount, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = new { amount };
            var response = await _httpClient.PostAsJsonAsync(
                $"{BaseUrl}/v1/payments/{transactionId}/refunds", payload, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return PaymentGatewayResult.Failed("Refund failed.");

            return PaymentGatewayResult.Approved(transactionId, "Refunded");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refunding payment {TransactionId}", transactionId);
            return PaymentGatewayResult.Failed("Unexpected error during refund.");
        }
    }

    public async Task<PaymentGatewayResult> GetTransactionStatusAsync(
        string transactionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"{BaseUrl}/v1/payments/{transactionId}", cancellationToken);

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!response.IsSuccessStatusCode)
                return PaymentGatewayResult.Failed("Could not retrieve transaction status.");

            var doc = JsonDocument.Parse(content);
            var status = doc.RootElement.GetProperty("status").GetString();

            return status switch
            {
                "approved" => PaymentGatewayResult.Approved(transactionId, content),
                "pending" or "in_process" => PaymentGatewayResult.Pending(transactionId, content),
                _ => PaymentGatewayResult.Failed($"Status: {status}")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching transaction status {TransactionId}", transactionId);
            return PaymentGatewayResult.Failed("Unexpected error fetching status.");
        }
    }

    private static object BuildPaymentPayload(PaymentRequest request)
    {
        var paymentType = request.Method switch
        {
            PaymentMethod.Pix => new { payment_method_id = "pix" },
            PaymentMethod.BankSlip => new { payment_method_id = "bolbradesco" },
            PaymentMethod.CreditCard => new { payment_method_id = "credit_card" },
            PaymentMethod.DebitCard => new { payment_method_id = "debit_card" },
            _ => new { payment_method_id = "pix" }
        };

        return new
        {
            transaction_amount = request.Amount,
            description = $"Pedido {request.OrderNumber}",
            payment_method_id = paymentType.payment_method_id,
            installments = request.InstallmentCount,
            token = request.CardToken,
            external_reference = request.OrderNumber
        };
    }
}

/// <summary>Result padronizado do gateway de pagamento.</summary>
public class PaymentGatewayResult
{
    public bool IsSuccessful { get; private set; }
    public bool IsPending { get; private set; }
    public string? TransactionId { get; private set; }
    public string? GatewayResponse { get; private set; }
    public string? ErrorMessage { get; private set; }

    public static PaymentGatewayResult Approved(string transactionId, string response) =>
        new() { IsSuccessful = true, TransactionId = transactionId, GatewayResponse = response };

    public static PaymentGatewayResult Pending(string transactionId, string response) =>
        new() { IsPending = true, TransactionId = transactionId, GatewayResponse = response };

    public static PaymentGatewayResult Failed(string error) =>
        new() { ErrorMessage = error };
}
