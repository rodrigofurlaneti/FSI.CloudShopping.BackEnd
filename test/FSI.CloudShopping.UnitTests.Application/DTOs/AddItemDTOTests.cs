using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class AddItemDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Constructor_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var expectedProductId = 10;
            var expectedQuantity = 2;

            // Act
            var dto = new AddItemDTO(expectedProductId, expectedQuantity);

            // Assert
            dto.ProductId.Should().Be(expectedProductId);
            dto.Quantity.Should().Be(expectedQuantity);
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Records_WithSameValues_ShouldBeEqual()
        {
            // Arrange
            var dto1 = new AddItemDTO(1, 5);
            var dto2 = new AddItemDTO(1, 5);

            // Assert
            dto1.Should().Be(dto2);
            (dto1 == dto2).Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Records_WithDifferentValues_ShouldNotBeEqual()
        {
            // Arrange
            var dto1 = new AddItemDTO(1, 5);
            var dto2 = new AddItemDTO(2, 5);

            // Assert
            dto1.Should().NotBe(dto2);
            (dto1 != dto2).Should().BeTrue();
        }
    }
}