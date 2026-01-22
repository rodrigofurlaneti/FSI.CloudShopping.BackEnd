using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment> ProcessPaymentAsync(Order order, PaymentMethod method)
        {
            var payment = new Payment(order.Id, method, order.TotalAmount);
            bool success = true;
            payment.Capture();
            await _paymentRepository.AddAsync(payment);
            return payment;
        }
    }
}