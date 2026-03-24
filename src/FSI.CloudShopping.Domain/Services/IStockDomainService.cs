namespace FSI.CloudShopping.Domain.Services;

public interface IStockDomainService
{
    /// <summary>
    /// Verifica se todos os itens têm estoque disponível suficiente.
    /// </summary>
    Task<bool> CheckAvailabilityAsync(
        IEnumerable<(int ProductId, int Quantity)> items,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Reserva estoque para todos os itens de forma atômica.
    /// Retorna false se qualquer item não tiver estoque suficiente.
    /// </summary>
    Task<bool> ReserveStockAsync(
        IEnumerable<(int ProductId, int Quantity)> items,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Libera estoque reservado (compensação em caso de falha no pedido).
    /// </summary>
    Task ReleaseStockAsync(
        IEnumerable<(int ProductId, int Quantity)> items,
        CancellationToken cancellationToken = default);
}
