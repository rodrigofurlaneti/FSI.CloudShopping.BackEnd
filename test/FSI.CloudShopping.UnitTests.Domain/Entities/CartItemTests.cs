using FluentAssertions;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.Entities
{
    public class CartItemTests
    {
        [Fact]
        public void UpdateQuantity_ShouldIncreaseTotalValue()
        {
            // Arrange
            var item = new CartItem(1, new Quantity(2), new Money(100));
            var extraQuantity = new Quantity(3);
            // Act
            item.UpdateQuantity(extraQuantity);
            // Assert
            item.Quantity.Value.Should().Be(5);
        }

        [Fact]
        public void TotalPrice_ShouldReturnCorrectCalculation()
        {
            // Arrange
            var price = new Money(150.50m);
            var qty = new Quantity(3);
            var item = new CartItem(1, qty, price);
            // Act
            var total = item.TotalPrice;
            // Assert
            total.Value.Should().Be(451.50m);
        }
    }
}