using AutoMapper;
using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Application.Services
{
    public class PaymentAppService : BaseAppService<Payment, PaymentDTO>, IPaymentAppService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentAppService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository,
            IMapper mapper) : base(paymentRepository, mapper)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentDTO> ProcessPaymentAsync(PaymentDTO paymentDto)
        {
            var order = await _orderRepository.GetByIdAsync(paymentDto.OrderId);
            if (order == null)
                throw new DomainException("Pedido não encontrado para processar pagamento.");
            var payment = new Payment(
                order.Id,
                PaymentMethod.FromString(paymentDto.PaymentMethod),
                new Money(paymentDto.Amount));

            try
            {
                payment.Capture(); 
            }
            catch (Exception)
            {
                payment.Fail();
                throw;
            }
            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();
            return Mapper.Map<PaymentDTO>(payment);
        }

        public async Task<PaymentDTO?> GetByOrderIdAsync(int orderId)
        {
            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
            return Mapper.Map<PaymentDTO>(payment);
        }
    }
}