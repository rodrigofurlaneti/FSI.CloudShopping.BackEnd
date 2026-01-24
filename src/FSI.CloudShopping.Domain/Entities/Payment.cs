using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Payment : Entity
    {
        public int OrderId { get; private set; }
        public PaymentMethod Method { get; private set; }
        public Money Amount { get; private set; }
        public PaymentStatus Status { get; private set; }
        protected Payment() { }

        public Payment(int orderId, PaymentMethod method, Money amount)
        {
            if (orderId <= 0)
                throw new DomainException("O ID do pedido é obrigatório para um pagamento.");

            if (amount.Value <= 0)
                throw new DomainException("O valor do pagamento deve ser maior que zero.");

            OrderId = orderId;
            Method = method;
            Amount = amount;
            Status = PaymentStatus.Pending;
        }

        public void Capture()
        {
            if (Status != PaymentStatus.Pending)
                throw new DomainException("Apenas pagamentos pendentes podem ser capturados.");

            Status = PaymentStatus.Captured;
        }

        public void Fail() => Status = PaymentStatus.Failed;

        public void Refund()
        {
            if (Status != PaymentStatus.Captured)
                throw new DomainException("Apenas pagamentos capturados podem ser reembolsados.");

            Status = PaymentStatus.Refunded;
        }
    }
}