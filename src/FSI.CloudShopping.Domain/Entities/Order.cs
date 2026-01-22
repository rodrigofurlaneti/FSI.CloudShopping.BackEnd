using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Order : Entity
    {
        public int CustomerId { get; private set; }
        public OrderStatus Status { get; private set; }
        public Money TotalAmount { get; private set; }
        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items;
        protected Order() { }
        public Order(int customerId, IEnumerable<CartItem> cartItems)
        {
            CustomerId = customerId;
            Status = OrderStatus.Pending;
            foreach (var item in cartItems)
                _items.Add(new OrderItem(item.ProductId, item.Quantity, item.UnitPrice));
            TotalAmount = CalculateTotal();
        }
        public Order(int customerId, List<OrderItem> items)
        {
            CustomerId = customerId;
            Status = OrderStatus.Pending;
            _items = items;
            TotalAmount = CalculateTotal();
        }
        private Money CalculateTotal()
        {
            decimal total = _items.Sum(x => x.SubTotal().Value);
            return new Money(total);
        }
    }
}
