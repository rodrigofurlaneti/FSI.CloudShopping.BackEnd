using FSI.CloudShopping.Application.DTOs;
using FSI.CloudShopping.Application.Interfaces;
using FSI.CloudShopping.Application.Mappings; 
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.Interfaces;
using FSI.CloudShopping.Domain.Interfaces.Services;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Application.Services
{
    public class PaymentAppService : BaseAppService<PaymentDTO, Payment>, IPaymentAppService
    {
        private readonly IPaymentService _paymentDomainService;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        public PaymentAppService(
            IPaymentRepository repository,
            IPaymentService paymentDomainService,
            IOrderRepository orderRepository) : base(repository)
        {
            _paymentDomainService = paymentDomainService;
            _orderRepository = orderRepository;
            _paymentRepository = repository;
        }
        public async Task<PaymentDTO> ProcessPaymentAsync(PaymentDTO paymentDto)
        {
            var order = await _orderRepository.GetByIdAsync(paymentDto.OrderId)
                        ?? throw new ApplicationException("Pedido não encontrado para pagamento.");
            var paymentMethod = PaymentMethod.FromString(paymentDto.Method);
            var payment = await _paymentDomainService.ProcessPaymentAsync(order, paymentMethod);
            await _paymentRepository.SaveChangesAsync();
            return MapToDto(payment);
        }
        public async Task<PaymentDTO?> GetByOrderIdAsync(int orderId)
        {
            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);
            return payment == null ? null : MapToDto(payment);
        }
        protected override Payment MapToEntity(PaymentDTO dto) => PaymentMapping.MapToEntity(dto);
        protected override PaymentDTO MapToDto(Payment entity) => PaymentMapping.MapToDto(entity);
    }
}