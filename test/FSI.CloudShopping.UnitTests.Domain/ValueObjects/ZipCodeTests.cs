using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class ZipCodeTests
    {
        [Fact]
        public void Should_Create_ZipCode_When_Value_Is_Valid()
        {
            var zipCode = new ZipCode("12345678");

            zipCode.Value.Should().Be("12345678");
        }

        [Fact]
        public void Should_Remove_Non_Digit_Characters()
        {
            var zipCode = new ZipCode("12345-678");

            zipCode.Value.Should().Be("12345678");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Throw_Exception_When_Value_Is_Empty(string value)
        {
            var act = () => new ZipCode(value);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("ZipCode cannot be empty.");
        }

        [Theory]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("12.345-67")]
        public void Should_Throw_Exception_When_Length_Is_Invalid(string value)
        {
            var act = () => new ZipCode(value);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("ZipCode must have 8 digits.");
        }

        [Fact]
        public void Formatted_Should_Return_ZipCode_With_Hyphen()
        {
            var zipCode = new ZipCode("12345678");

            var formatted = zipCode.Formatted();

            formatted.Should().Be("12345-678");
        }

        [Fact]
        public void ToString_Should_Return_Clean_Value()
        {
            var zipCode = new ZipCode("12345-678");

            zipCode.ToString().Should().Be("12345678");
        }
    }
}
