using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void DebitStock_ShouldReduceQuantity_WhenStockIsAvailable()
        {
            // Arrange
            var product = new Product(
                new SKU("LAP-DELL-G15"),
                "Notebook Dell G15",
                new Money(5000),
                new Quantity(10));
            // Act
            product.DebitStock(new Quantity(3));
            // Assert
            product.Stock.Value.Should().Be(7);
        }
        [Fact]
        public void DebitStock_ShouldThrowDomainException_WhenStockIsInsufficient()
        {
            // Arrange
            var product = new Product(
                new SKU("IPHONE-15"),
                "iPhone 15",
                new Money(7000),
                new Quantity(5));
            // Act
            Action act = () => product.DebitStock(new Quantity(6));
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Estoque insuficiente para o produto iPhone 15.");
        }
        [Fact]
        public void UpdatePrice_ShouldChangeProductPrice()
        {
            // Arrange
            var product = new Product(
                new SKU("TV-SAMSUNG-50"),
                "Smart TV 50",
                new Money(2500),
                new Quantity(20));
            var newPrice = new Money(2300);
            // Act
            product.UpdatePrice(newPrice);
            // Assert
            product.Price.Should().Be(newPrice);
        }
    }
}