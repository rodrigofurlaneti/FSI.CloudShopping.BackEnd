namespace FSI.CloudShopping.Domain.Interfaces;

using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Enums;

public interface IPaymentRepository : IRepository<Payment, int>
{
    Task<Payment?> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);
    Task<Payment?> GetByTransactionIdAsync(string transactionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status, CancellationToken cancellationToken = default);
}
