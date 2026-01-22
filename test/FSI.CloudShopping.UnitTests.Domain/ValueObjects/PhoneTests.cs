using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class PhoneTests
    {
        [Theory]
        [InlineData("(11) 99999-8888", "11999998888")]
        [InlineData("11999998888", "11999998888")]
        [InlineData("11 4444-5555", "1144445555")]
        public void Constructor_ShouldSanitizeAndCreatePhone_WhenInputIsValid(string input, string expected)
        {
            // Act
            var phone = new Phone(input);
            // Assert
            phone.Number.Should().Be(expected);
        }
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Constructor_ShouldThrowDomainException_WhenPhoneIsEmpty(string? input)
        {
            // Act
            Action act = () => new Phone(input!);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Phone number is required.");
        }
        [Theory]
        [InlineData("123456789")]
        [InlineData("119999988887")]
        [InlineData("ABC11999998")]
        public void Constructor_ShouldThrowDomainException_WhenLengthIsInvalid(string input)
        {
            // Act
            Action act = () => new Phone(input);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Invalid phone number length.");
        }
        [Fact]
        public void Phone_ShouldSupportValueEquality()
        {
            // Arrange
            var phone1 = new Phone("(11) 98888-7777");
            var phone2 = new Phone("11988887777");
            // Assert
            phone1.Should().Be(phone2);
        }
    }
}
