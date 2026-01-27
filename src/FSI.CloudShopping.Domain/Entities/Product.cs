using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.Domain.Entities
{
    public class Product : Entity
    {
        public SKU Sku { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money Price { get; private set; }
        public Quantity Stock { get; private set; }
        public List<string> Images { get; private set; } = new List<string>(); 
        protected Product() { }
        public Product(SKU sku, string name, string description, Money price, Quantity stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("O nome do produto é obrigatório.");

            Sku = sku;
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
        }

        public void AddImages(IEnumerable<string> paths)
        {
            if (paths == null) return;
            Images.AddRange(paths);
        }

        public void UpdateStock(Quantity newQuantity)
        {
            Stock = newQuantity;
        }

        public void UpdatePrice(Money newPrice)
        {
            Price = newPrice;
        }

        public void DebitStock(Quantity quantity)
        {
            if (Stock.Value < quantity.Value)
                throw new DomainException($"Estoque insuficiente para o produto {Name}.");
            Stock = new Quantity(Stock.Value - quantity.Value);
        }

        public void CreditStock(Quantity quantity)
        {
            Stock = new Quantity(Stock.Value + quantity.Value);
        }
    }
}