using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Order order, PaymentMethod method);
    }
}
