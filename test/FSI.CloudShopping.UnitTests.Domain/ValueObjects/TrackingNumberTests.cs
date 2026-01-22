using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;
namespace FSI.CloudShopping.Domain.UnitTests.ValueObjects
{
    public class TrackingNumberTests
    {
        [Theory]
        [InlineData("br123456789br", "BR123456789BR")]
        [InlineData("  fixed123  ", "FIXED123")]
        [InlineData("track-999-xyz", "TRACK-999-XYZ")]
        public void Constructor_ShouldNormalizeAndCreate_WhenInputIsValid(string input, string expected)
        {
            // Act
            var tracking = new TrackingNumber(input);
            // Assert
            tracking.Code.Should().Be(expected);
        }
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_ShouldThrowDomainException_WhenCodeIsEmpty(string? input)
        {
            // Act
            Action act = () => new TrackingNumber(input!);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Tracking number cannot be empty.");
        }

        [Fact]
        public void TrackingNumber_ShouldSupportValueEquality()
        {
            // Arrange
            var t1 = new TrackingNumber("AA123456789BR");
            var t2 = new TrackingNumber("aa123456789br ");
            // Assert
            t1.Should().Be(t2);
        }
    }
}