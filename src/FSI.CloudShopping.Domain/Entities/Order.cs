using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Order : Entity
    {
        public int CustomerId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public Money TotalAmount { get; private set; }
        public int ShippingAddressId { get; private set; }

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items;
        protected Order() { }
        public Order(int customerId, int shippingAddressId, IEnumerable<CartItem> cartItems)
        {
            if (customerId <= 0) throw new DomainException("Cliente inválido.");
            if (shippingAddressId <= 0) throw new DomainException("Endereço de entrega é obrigatório.");
            if (!cartItems.Any()) throw new DomainException("O pedido deve ter pelo menos um item.");

            CustomerId = customerId;
            ShippingAddressId = shippingAddressId;
            OrderDate = DateTime.Now;
            Status = OrderStatus.Pending;

            foreach (var item in cartItems)
            {
                _items.Add(new OrderItem(item.ProductId, item.Quantity, item.UnitPrice));
            }

            TotalAmount = CalculateTotal();
        }
        private Money CalculateTotal()
        {
            decimal total = _items.Sum(x => x.UnitPrice.Value * x.Quantity.Value);
            return new Money(total);
        }
        public void ChangeStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }
    }
}