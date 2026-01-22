using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class CartItem : Entity
    {
        public int CartId { get; private set; }
        public int ProductId { get; private set; }
        public Quantity Quantity { get; private set; }
        public Money UnitPrice { get; private set; }
        protected CartItem() { }
        public CartItem(int productId, Quantity quantity, Money unitPrice)
        {
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
        public void UpdateQuantity(Quantity additionalQuantity)
        {
            Quantity = new Quantity(Quantity.Value + additionalQuantity.Value);
        }
        public Money TotalPrice => UnitPrice.Multiply(Quantity.Value);
    }
}