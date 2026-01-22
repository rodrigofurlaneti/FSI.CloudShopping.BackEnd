using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class PaymentMethodTests
    {
        [Theory]
        [InlineData("CreditCard", "CreditCard")]
        [InlineData("Pix", "Pix")]
        [InlineData("Invoice", "Invoice")]
        public void FromString_ShouldReturnCorrectPaymentMethod_WhenValueIsValid(string input, string expectedDescription)
        {
            // Act
            var result = PaymentMethod.FromString(input);
            // Assert
            result.Description.Should().Be(expectedDescription);
        }

        [Fact]
        public void FromString_ShouldThrowArgumentException_WhenValueIsInvalid()
        {
            // Arrange
            var invalidMethod = "Bitcoin";
            // Act
            Action act = () => PaymentMethod.FromString(invalidMethod);
            // Assert
            act.Should().Throw<ArgumentException>()
               .WithMessage("Invalid Payment Method");
        }

        [Fact]
        public void StaticProperties_ShouldReturnCorrectInstances()
        {
            // Assert
            PaymentMethod.CreditCard.Description.Should().Be("CreditCard");
            PaymentMethod.Pix.Description.Should().Be("Pix");
            PaymentMethod.Invoice.Description.Should().Be("Invoice");
        }

        [Fact]
        public void PaymentMethod_ShouldSupportValueEquality()
        {
            // Arrange
            var method1 = PaymentMethod.FromString("Pix");
            var method2 = PaymentMethod.Pix;
            // Assert
            method1.Should().Be(method2);
            (method1 == method2).Should().BeTrue();
        }
    }
}
