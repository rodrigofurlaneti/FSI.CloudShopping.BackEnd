using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class CartItemDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Constructor_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var expectedProductId = 501;
            var expectedProductName = "Smartphone Gamer X";
            var expectedQuantity = 2;
            var expectedUnitPrice = 2500.00m;
            var expectedTotalPrice = 5000.00m;

            // Act
            var dto = new CartItemDTO(
                expectedProductId,
                expectedProductName,
                expectedQuantity,
                expectedUnitPrice,
                expectedTotalPrice);

            // Assert
            dto.ProductId.Should().Be(expectedProductId);
            dto.ProductName.Should().Be(expectedProductName);
            dto.Quantity.Should().Be(expectedQuantity);
            dto.UnitPrice.Should().Be(expectedUnitPrice);
            dto.TotalPrice.Should().Be(expectedTotalPrice);
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Records_WithSameValues_ShouldBeEqual()
        {
            // Arrange
            var dto1 = new CartItemDTO(1, "Item A", 1, 10.0m, 10.0m);
            var dto2 = new CartItemDTO(1, "Item A", 1, 10.0m, 10.0m);

            // Assert
            dto1.Should().Be(dto2);
            (dto1 == dto2).Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Deconstructor_ShouldExtractValuesCorrectly()
        {
            // Arrange
            var dto = new CartItemDTO(10, "Mouse", 1, 50.0m, 50.0m);

            // Act (Testando o desconstrutor nativo de positional records)
            var (id, name, qty, price, total) = dto;

            // Assert
            id.Should().Be(10);
            name.Should().Be("Mouse");
            total.Should().Be(50.0m);
        }
    }
}