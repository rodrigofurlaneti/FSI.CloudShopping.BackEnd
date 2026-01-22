using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;
namespace FSI.CloudShopping.Domain.UnitTests.ValueObjects
{
    public class PaymentStatusTests
    {
        [Theory]
        [InlineData("Pending", "Pending")]
        [InlineData("Captured", "Captured")]
        [InlineData("Refunded", "Refunded")]
        public void FromString_ShouldReturnCorrectStatus_WhenValueIsValid(string input, string expectedDescription)
        {
            // Act
            var result = PaymentStatus.FromString(input);
            // Assert
            result.Description.Should().Be(expectedDescription);
        }

        [Fact]
        public void FromString_ShouldThrowArgumentException_WhenValueIsInvalid()
        {
            // Arrange
            var invalidStatus = "AuthorizedButNotCaptured";
            // Act
            Action act = () => PaymentStatus.FromString(invalidStatus);
            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Invalid Payment Status");
        }

        [Fact]
        public void StaticProperties_ShouldMatchExpectedDescriptions()
        {
            // Assert
            PaymentStatus.Pending.Description.Should().Be("Pending");
            PaymentStatus.Captured.Description.Should().Be("Captured");
            PaymentStatus.Refunded.Description.Should().Be("Refunded");
        }

        [Fact]
        public void PaymentStatus_ShouldSupportValueEquality()
        {
            // Arrange
            var status1 = PaymentStatus.FromString("Captured");
            var status2 = PaymentStatus.Captured;
            // Assert
            status1.Should().Be(status2);
            (status1 == status2).Should().BeTrue();
        }
    }
}