using FSI.CloudShopping.Application.DTOs;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.UnitTests.Application.DTOs
{
    public class PaymentDTOTests
    {
        [Fact]
        [Trait("Category", "DTO")]
        public void Properties_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var dto = new PaymentDTO();
            var expectedOrderId = 1001;
            var expectedMethod = "CreditCard";
            var expectedAmount = 250.75m;
            var expectedStatus = "Approved";

            // Act
            dto.OrderId = expectedOrderId;
            dto.Method = expectedMethod;
            dto.Amount = expectedAmount;
            dto.Status = expectedStatus;

            // Assert
            dto.OrderId.Should().Be(expectedOrderId);
            dto.Method.Should().Be(expectedMethod);
            dto.Amount.Should().Be(expectedAmount);
            dto.Status.Should().Be(expectedStatus);
        }

        [Fact]
        [Trait("Category", "DTO")]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var dto = new PaymentDTO();

            // Assert
            dto.OrderId.Should().Be(0);
            dto.Method.Should().BeEmpty();
            dto.Amount.Should().Be(0);
            dto.Status.Should().BeEmpty();
        }
    }
}