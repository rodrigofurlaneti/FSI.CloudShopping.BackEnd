namespace FSI.CloudShopping.Application.Sagas.Steps;

using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Sagas;
using Microsoft.Extensions.Logging;

/// <summary>
/// SAGA Step 1 — Reserve Stock.
/// Decrements available stock for each item in the order.
/// Compensation: releases the reserved stock if a later step fails.
/// </summary>
public sealed class ReserveStockStep : ISagaStep<OrderCheckoutSagaState>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ReserveStockStep> _logger;

    public string StepName => "ReserveStock";

    public ReserveStockStep(IProductRepository productRepository, ILogger<ReserveStockStep> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task ExecuteAsync(OrderCheckoutSagaState state, CancellationToken cancellationToken = default)
    {
        foreach (var item in state.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken)
                ?? throw new InvalidOperationException($"Product {item.ProductId} not found.");

            product.ReserveStock(item.Quantity);
            await _productRepository.UpdateAsync(product, cancellationToken);
            state.ReservedStockIds.Add(Guid.NewGuid()); // reservation token per item
            _logger.LogInformation("Stock reserved for product {ProductId} x{Qty}", item.ProductId, item.Quantity);
        }

        await _productRepository.SaveChangesAsync(cancellationToken);
        state.StockReserved = true;
    }

    public async Task CompensateAsync(OrderCheckoutSagaState state, CancellationToken cancellationToken = default)
    {
        _logger.LogWarning("Compensating: releasing stock reservations for SAGA {SagaId}", state.SagaId);

        foreach (var item in state.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            if (product is null) continue;

            product.ReleaseReservedStock(item.Quantity);
            await _productRepository.UpdateAsync(product, cancellationToken);
        }

        await _productRepository.SaveChangesAsync(cancellationToken);
        state.StockReserved = false;
        state.ReservedStockIds.Clear();
    }
}
