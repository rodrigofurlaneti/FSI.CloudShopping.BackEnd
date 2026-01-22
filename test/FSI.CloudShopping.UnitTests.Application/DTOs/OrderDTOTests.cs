using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using System.Linq;
namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class OrderDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Properties_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var dto = new OrderDTO();
            var expectedId = 500;
            var expectedCustomerId = 10;
            var expectedStatus = "Paid";
            var expectedTotal = 1500.00m;
            var expectedItems = new List<OrderItemDTO>
            {
                new OrderItemDTO { ProductId = 1, Quantity = 1, UnitPrice = 1500.00m }
            };

            // Act
            dto.Id = expectedId;
            dto.CustomerId = expectedCustomerId;
            dto.Status = expectedStatus;
            dto.TotalAmount = expectedTotal;
            dto.Items = expectedItems;

            // Assert
            dto.Id.Should().Be(expectedId);
            dto.CustomerId.Should().Be(expectedCustomerId);
            dto.Status.Should().Be(expectedStatus);
            dto.TotalAmount.Should().Be(expectedTotal);
            dto.Items.Should().HaveCount(1);
            dto.Items.First().ProductId.Should().Be(1);
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Constructor_ShouldInitializeWithEmptyItemList()
        {
            // Act
            var dto = new OrderDTO();

            // Assert
            dto.Items.Should().NotBeNull();
            dto.Items.Should().BeEmpty();
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void ListProperty_ShouldAllowAddingItems()
        {
            // Arrange
            var dto = new OrderDTO();
            var item = new OrderItemDTO { ProductId = 99, Quantity = 5 };

            // Act
            dto.Items.Add(item);

            // Assert
            dto.Items.Should().Contain(item);
            dto.Items.Should().HaveCount(1);
        }
    }
}