using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class ZipCodeTests
    {
        [Theory]
        [InlineData("01001-000", "01001000")]
        [InlineData("01001000", "01001000")]
        [InlineData(" 12345-678 ", "12345678")]
        public void Constructor_ShouldSanitizeAndCreate_WhenInputIsValid(string input, string expected)
        {
            // Act
            var zipCode = new ZipCode(input);
            // Assert
            zipCode.Value.Should().Be(expected);
        }
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_ShouldThrowDomainException_WhenZipCodeIsEmpty(string? input)
        {
            // Act
            Action act = () => new ZipCode(input!);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("ZipCode cannot be empty.");
        }
        [Theory]
        [InlineData("1234567")]
        [InlineData("123456789")]
        [InlineData("ABC12345")]
        public void Constructor_ShouldThrowDomainException_WhenLengthIsInvalid(string input)
        {
            // Act
            Action act = () => new ZipCode(input);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("ZipCode must have 8 digits.");
        }
        [Fact]
        public void Formatted_ShouldReturnZipCodeWithHyphen()
        {
            // Arrange
            var zipCode = new ZipCode("01001000");
            // Act
            var result = zipCode.Formatted();
            // Assert
            result.Should().Be("01001-000");
        }

        [Fact]
        public void ZipCode_ShouldSupportValueEquality()
        {
            // Arrange
            var zip1 = new ZipCode("01310-100");
            var zip2 = new ZipCode("01310100");
            // Assert
            zip1.Should().Be(zip2);
            (zip1 == zip2).Should().BeTrue();
        }
    }
}