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
            OrderId = orderId;
            Method = method;
            Amount = amount;
            Status = PaymentStatus.Pending; 
        }

        public void Capture()
        {
            Status = PaymentStatus.Captured;
        }

        public void Refund() => Status = PaymentStatus.Refunded;
    }
}