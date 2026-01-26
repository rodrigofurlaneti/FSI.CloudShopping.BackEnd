using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class PaymentStatusTests
    {
        #region Static Instances

        [Fact]
        public void Pending_Should_Have_Correct_Description()
        {
            PaymentStatus.Pending.Description.Should().Be("Pending");
        }

        [Fact]
        public void Captured_Should_Have_Correct_Description()
        {
            PaymentStatus.Captured.Description.Should().Be("Captured");
        }

        [Fact]
        public void Refunded_Should_Have_Correct_Description()
        {
            PaymentStatus.Refunded.Description.Should().Be("Refunded");
        }

        [Fact]
        public void Failed_Should_Have_Correct_Description()
        {
            PaymentStatus.Failed.Description.Should().Be("Failed");
        }

        #endregion

        #region FromString - Valid

        [Theory]
        [InlineData("Pending")]
        [InlineData("Captured")]
        [InlineData("Refunded")]
        [InlineData("Failed")]
        public void FromString_Should_Return_Correct_PaymentStatus(string value)
        {
            var result = PaymentStatus.FromString(value);

            result.Description.Should().Be(value);
        }

        [Theory]
        [InlineData(" Pending ")]
        [InlineData("Captured ")]
        [InlineData(" Refunded")]
        [InlineData("  Failed  ")]
        public void FromString_Should_Ignore_Whitespace(string value)
        {
            var result = PaymentStatus.FromString(value);

            result.Should().NotBeNull();
        }

        #endregion

        #region FromString - Invalid

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Approved")]
        [InlineData("Processing")]
        public void FromString_Should_Throw_When_Value_Is_Invalid(string value)
        {
            Action act = () => PaymentStatus.FromString(value);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage($"Status de pagamento inválido: {value}");
        }

        [Fact]
        public void FromString_Should_Throw_When_Value_Is_Null()
        {
            Action act = () => PaymentStatus.FromString(null!);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("Status de pagamento inválido: ");
        }

        #endregion

        #region Equality (Record)

        [Fact]
        public void PaymentStatus_Should_Be_Equal_When_Description_Is_Same()
        {
            var status1 = PaymentStatus.Pending;
            var status2 = PaymentStatus.FromString("Pending");

            status1.Should().Be(status2);
            status1.GetHashCode().Should().Be(status2.GetHashCode());
        }

        [Fact]
        public void PaymentStatus_Should_Not_Be_Equal_When_Description_Is_Different()
        {
            PaymentStatus.Pending.Should().NotBe(PaymentStatus.Failed);
        }

        #endregion

        #region ToString

        [Fact]
        public void ToString_Should_Return_Description()
        {
            PaymentStatus.Captured.ToString().Should().Be("Captured");
        }

        #endregion
    }
}
