using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class PaymentMethodTests
    {
        #region Static Instances

        [Fact]
        public void CreditCard_Should_Have_Correct_Description()
        {
            PaymentMethod.CreditCard.Description.Should().Be("CreditCard");
        }

        [Fact]
        public void Pix_Should_Have_Correct_Description()
        {
            PaymentMethod.Pix.Description.Should().Be("Pix");
        }

        [Fact]
        public void Invoice_Should_Have_Correct_Description()
        {
            PaymentMethod.Invoice.Description.Should().Be("Invoice");
        }

        #endregion

        #region FromString - Valid

        [Theory]
        [InlineData("CreditCard")]
        [InlineData("Pix")]
        [InlineData("Invoice")]
        public void FromString_Should_Return_Correct_PaymentMethod(string value)
        {
            // Act
            var result = PaymentMethod.FromString(value);

            // Assert
            result.Description.Should().Be(value);
        }

        #endregion

        #region FromString - Invalid

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Cash")]
        [InlineData("Debit")]
        public void FromString_Should_Throw_When_Value_Is_Invalid(string value)
        {
            Action act = () => PaymentMethod.FromString(value);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("Invalid Payment Method");
        }

        [Fact]
        public void FromString_Should_Throw_When_Value_Is_Null()
        {
            Action act = () => PaymentMethod.FromString(null!);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("Invalid Payment Method");
        }

        #endregion

        #region Equality (Record)

        [Fact]
        public void PaymentMethod_Should_Be_Equal_When_Description_Is_Same()
        {
            var method1 = PaymentMethod.CreditCard;
            var method2 = PaymentMethod.FromString("CreditCard");

            method1.Should().Be(method2);
            method1.GetHashCode().Should().Be(method2.GetHashCode());
        }

        [Fact]
        public void PaymentMethod_Should_Not_Be_Equal_When_Description_Is_Different()
        {
            PaymentMethod.CreditCard.Should().NotBe(PaymentMethod.Pix);
        }

        #endregion
    }
}
