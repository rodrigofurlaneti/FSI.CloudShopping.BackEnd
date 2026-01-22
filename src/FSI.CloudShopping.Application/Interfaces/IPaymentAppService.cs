using FSI.CloudShopping.Application.DTOs;
namespace FSI.CloudShopping.Application.Interfaces
{
    public interface IPaymentAppService : IBaseAppService<PaymentDTO>
    {
        Task<PaymentDTO> ProcessPaymentAsync(PaymentDTO paymentDto);
        Task<PaymentDTO?> GetByOrderIdAsync(int orderId);
    }
}