using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class CartDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Init_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var expectedId = 1;
            var expectedToken = Guid.NewGuid().ToString();
            var expectedCustomerId = 42;
            var expectedTotal = 150.50m;
            var items = new List<CartItemDTO>
            {
                new CartItemDTO(1, "Produto Teste", 2, 75.25m, 150.50m)
            };

            // Act
            var dto = new CartDTO
            {
                Id = expectedId,
                VisitorToken = expectedToken,
                CustomerId = expectedCustomerId,
                Items = items,
                TotalAmount = expectedTotal
            };

            // Assert
            dto.Id.Should().Be(expectedId);
            dto.VisitorToken.Should().Be(expectedToken);
            dto.CustomerId.Should().Be(expectedCustomerId);
            dto.Items.Should().HaveCount(1);
            dto.TotalAmount.Should().Be(expectedTotal);
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Constructor_ShouldInitializeWithEmptyList()
        {
            // Act
            var dto = new CartDTO();

            // Assert
            dto.Items.Should().NotBeNull();
            dto.Items.Should().BeEmpty();
        }
    }
}