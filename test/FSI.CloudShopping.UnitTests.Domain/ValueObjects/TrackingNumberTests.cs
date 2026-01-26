using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class TrackingNumberTests
    {
        [Fact]
        public void Should_Create_TrackingNumber_When_Code_Is_Valid()
        {
            var tracking = new TrackingNumber("br123456789");

            tracking.Code.Should().Be("BR123456789");
        }

        [Fact]
        public void Should_Trim_And_Uppercase_Code()
        {
            var tracking = new TrackingNumber("  ab987654321  ");

            tracking.Code.Should().Be("AB987654321");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Throw_Exception_When_Code_Is_Invalid(string code)
        {
            var act = () => new TrackingNumber(code);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Tracking number cannot be empty.");
        }

        [Fact]
        public void ToString_Should_Return_Code()
        {
            var tracking = new TrackingNumber("trk-0001");

            tracking.ToString().Should().Be("TRK-0001");
        }
    }
}
