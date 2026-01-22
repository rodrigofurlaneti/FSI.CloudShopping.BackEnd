using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Product : Entity
    {
        public SKU Sku { get; private set; }
        public string Name { get; private set; }
        public Money Price { get; private set; }
        public Quantity Stock { get; private set; }
        protected Product() { }
        public Product(SKU sku, string name, Money price, Quantity stock)
        {
            Sku = sku;
            Name = name;
            Price = price;
            Stock = stock;
        }
        public void UpdatePrice(Money newPrice) => Price = newPrice;
        public void DebitStock(Quantity quantity)
        {
            if (Stock.Value < quantity.Value)
                throw new DomainException($"Estoque insuficiente para o produto {Name}.");
            Stock = new Quantity(Stock.Value - quantity.Value);
        }
    }
}