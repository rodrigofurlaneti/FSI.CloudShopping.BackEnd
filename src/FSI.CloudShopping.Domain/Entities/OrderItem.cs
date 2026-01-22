using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class OrderItem : Entity
    {
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public Quantity Quantity { get; private set; }
        public Money UnitPrice { get; private set; }
        protected OrderItem() { }
        public OrderItem(int productId, Quantity quantity, Money unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
        public Money SubTotal() => UnitPrice.Multiply(Quantity.Value);
    }
}
