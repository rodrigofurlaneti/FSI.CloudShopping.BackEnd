using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Core;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class PhoneTests
    {
        [Fact]
        public void Should_Create_Phone_When_Number_Is_Valid_With_11_Digits()
        {
            var phone = new Phone("(11) 91234-5678");

            phone.Number.Should().Be("11912345678");
        }

        [Fact]
        public void Should_Create_Phone_When_Number_Is_Valid_With_10_Digits()
        {
            var phone = new Phone("11 1234-5678");

            phone.Number.Should().Be("1112345678");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Should_Throw_Exception_When_Number_Is_Null_Or_Empty(string number)
        {
            var act = () => new Phone(number);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Phone number is required.");
        }

        [Theory]
        [InlineData("123456789")]      // 9 dígitos
        [InlineData("123456789012")]   // 12 dígitos
        public void Should_Throw_Exception_When_Number_Length_Is_Invalid(string number)
        {
            var act = () => new Phone(number);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Invalid phone number length.");
        }

        [Fact]
        public void Should_Remove_Non_Numeric_Characters()
        {
            var phone = new Phone("(11) 91234-5678");
            phone.Number.Should().Be("11912345678");
        }

        [Fact]
        public void ToString_Should_Return_Number()
        {
            var phone = new Phone("11912345678");

            phone.ToString().Should().Be("11912345678");
        }
    }
}
