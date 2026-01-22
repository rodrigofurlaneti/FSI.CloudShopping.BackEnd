using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class ProductDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Init_ShouldSetAllPropertiesCorrectly()
        {
            // Arrange
            var expectedId = 1;
            var expectedSku = "PROD-001";
            var expectedName = "Smartphone XYZ";
            var expectedPrice = 1500.50m;
            var expectedStock = 10;
            var expectedIsActive = true;

            // Act
            var dto = new ProductDTO
            {
                Id = expectedId,
                Sku = expectedSku,
                Name = expectedName,
                Price = expectedPrice,
                Stock = expectedStock,
                IsActive = expectedIsActive
            };

            // Assert
            dto.Id.Should().Be(expectedId);
            dto.Sku.Should().Be(expectedSku);
            dto.Name.Should().Be(expectedName);
            dto.Price.Should().Be(expectedPrice);
            dto.Stock.Should().Be(expectedStock);
            dto.IsActive.Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var dto = new ProductDTO();

            // Assert
            dto.Sku.Should().BeEmpty();
            dto.Name.Should().BeEmpty();
            dto.Price.Should().Be(0);
            dto.Stock.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Records_WithSameValues_ShouldBeEqual()
        {
            // Arrange
            var dto1 = new ProductDTO { Id = 1, Sku = "ABC" };
            var dto2 = new ProductDTO { Id = 1, Sku = "ABC" };

            // Assert
            dto1.Should().Be(dto2);
            (dto1 == dto2).Should().BeTrue();
        }
    }
}