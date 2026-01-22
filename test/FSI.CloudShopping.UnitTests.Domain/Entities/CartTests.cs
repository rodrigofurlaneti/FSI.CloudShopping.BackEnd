using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Domain.Entities
{
    public class CartTests
    {
        [Fact]
        public void IsExpired_ShouldReturnTrue_WhenOverThirtyDays()
        {
            var cart = new Cart(new VisitorToken(Guid.NewGuid()));
            var prop = typeof(Cart).GetProperty("UpdatedAt");
            prop?.SetValue(cart, DateTime.UtcNow.AddDays(-32));
            cart.IsExpired().Should().BeTrue();
        }

        [Fact]
        public void AddOrUpdateItem_ShouldAccumulateQuantity_IfProductAlreadyExists()
        {
            // Arrange
            var cart = new Cart(1);
            var productId = 1;
            var price = new Money(100);
            // Act
            cart.AddOrUpdateItem(productId, new Quantity(1), price);
            cart.AddOrUpdateItem(productId, new Quantity(2), price);
            // Assert
            cart.Items.Should().HaveCount(1);
            cart.Items.First().Quantity.Value.Should().Be(3);
        }
    }
}