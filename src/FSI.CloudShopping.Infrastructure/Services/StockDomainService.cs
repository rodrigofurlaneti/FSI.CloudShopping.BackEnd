namespace FSI.CloudShopping.Infrastructure.Services;

using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Services;

/// <summary>
/// Serviço de domínio para gestão de estoque.
/// Garante que as operações de reserva e liberação sejam atômicas via UnitOfWork.
/// </summary>
public class StockDomainService : IStockDomainService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public StockDomainService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CheckAvailabilityAsync(
        IEnumerable<(int ProductId, int Quantity)> items,
        CancellationToken cancellationToken = default)
    {
        foreach (var (productId, quantity) in items)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null || product.GetAvailableStock() < quantity)
                return false;
        }
        return true;
    }

    public async Task<bool> ReserveStockAsync(
        IEnumerable<(int ProductId, int Quantity)> items,
        CancellationToken cancellationToken = default)
    {
        var productList = new List<(Domain.Entities.Product product, int quantity)>();

        foreach (var (productId, quantity) in items)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            if (product == null || product.GetAvailableStock() < quantity)
                return false;
            productList.Add((product, quantity));
        }

        foreach (var (product, quantity) in productList)
            product.ReserveStock(quantity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task ReleaseStockAsync(
        IEnumerable<(int ProductId, int Quantity)> items,
        CancellationToken cancellationToken = default)
    {
        foreach (var (productId, quantity) in items)
        {
            var product = await _productRepository.GetByIdAsync(productId, cancellationToken);
            product?.ReleaseReservedStock(quantity);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
