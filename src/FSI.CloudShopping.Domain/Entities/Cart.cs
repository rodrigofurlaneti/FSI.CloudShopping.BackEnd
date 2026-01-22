using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Cart : Entity
    {
        public VisitorToken? VisitorToken { get; private set; }
        public int? CustomerId { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        private readonly List<CartItem> _items = new();
        public IReadOnlyCollection<CartItem> Items => _items;
        protected Cart() { }
        public Cart(VisitorToken token)
        {
            VisitorToken = token;
            Touch();
        }
        public Cart(int customerId)
        {
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
                item.UpdateQuantity(quantity);
            }

            Touch();
        }
        public bool IsExpired() => UpdatedAt < DateTime.UtcNow.AddMonths(-1);
        private void Touch() => UpdatedAt = DateTime.UtcNow;
    }
}