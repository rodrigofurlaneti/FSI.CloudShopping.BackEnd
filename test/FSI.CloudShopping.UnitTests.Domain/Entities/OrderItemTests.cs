using FluentAssertions;
using Xunit;
using FSI.CloudShopping.Domain.Entities;
using FSI.CloudShopping.Domain.ValueObjects;

namespace FSI.CloudShopping.Domain.Tests.Entities
{
    public class OrderItemTests
    {
        [Fact]
        public void Constructor_Should_Set_All_Properties()
        {
            var productId = 10;
            var quantity = new Quantity(3);
            var unitPrice = new Money(25);

            var orderItem = new OrderItem(productId, quantity, unitPrice);

            orderItem.ProductId.Should().Be(productId);
            orderItem.Quantity.Should().Be(quantity);
            orderItem.UnitPrice.Should().Be(unitPrice);
        }

        [Fact]
        public void SubTotal_Should_Return_Correct_Amount()
        {
            var quantity = new Quantity(4);
            var unitPrice = new Money(12.5m);

            var orderItem = new OrderItem(1, quantity, unitPrice);

            var subtotal = orderItem.SubTotal();

            subtotal.Value.Should().Be(50m); // 4 * 12.5
        }
    }
}
