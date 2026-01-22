using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class OrderItemDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Properties_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var dto = new OrderItemDTO();
            var expectedProductId = 101;
            var expectedQuantity = 3;
            var expectedUnitPrice = 49.90m;

            // Act
            dto.ProductId = expectedProductId;
            dto.Quantity = expectedQuantity;
            dto.UnitPrice = expectedUnitPrice;

            // Assert
            dto.ProductId.Should().Be(expectedProductId);
            dto.Quantity.Should().Be(expectedQuantity);
            dto.UnitPrice.Should().Be(expectedUnitPrice);
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void OrderItemDTO_ShouldAllowDefaultInitialization()
        {
            // Act
            var dto = new OrderItemDTO();

            // Assert
            dto.ProductId.Should().Be(0);
            dto.Quantity.Should().Be(0);
            dto.UnitPrice.Should().Be(0);
        }
    }
}