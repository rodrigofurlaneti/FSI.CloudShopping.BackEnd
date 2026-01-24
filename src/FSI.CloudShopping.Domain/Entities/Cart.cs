using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Cart : Entity
    {
        public int CustomerId { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items;
        protected Cart() { }
        public Cart(int customerId)
        {
            if (customerId <= 0)
                throw new DomainException("O carrinho deve pertencer a um cliente válido.");

            CustomerId = customerId;
            Touch();
        }
        public void AddOrUpdateItem(int productId, Quantity quantity, Money price)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);
            if (item is null)
            {
                _items.Add(new CartItem(productId, quantity, price));
            }
            else
            {
                item.UpdateQuantity(new Quantity(item.Quantity.Value + quantity.Value));
            }

            Touch();
        }
        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
                Touch();
            }
        }
        public bool IsExpired() => UpdatedAt < DateTime.Now.AddDays(-30);
        private void Touch() => UpdatedAt = DateTime.Now;
        public Money GetTotal()
        {
            decimal total = _items.Sum(x => x.Quantity.Value * x.UnitPrice.Value);
            return new Money(total);
        }
    }
}