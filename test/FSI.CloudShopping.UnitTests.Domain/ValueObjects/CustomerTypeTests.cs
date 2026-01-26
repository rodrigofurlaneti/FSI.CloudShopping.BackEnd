using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class CustomerTypeTests
    {
        #region Static Instances

        [Fact]
        public void Guest_Should_Have_Correct_Code()
        {
            CustomerType.Guest.Code.Should().Be("Guest");
        }

        [Fact]
        public void Lead_Should_Have_Correct_Code()
        {
            CustomerType.Lead.Code.Should().Be("Lead");
        }

        [Fact]
        public void B2C_Should_Have_Correct_Code()
        {
            CustomerType.B2C.Code.Should().Be("B2C");
        }

        [Fact]
        public void B2B_Should_Have_Correct_Code()
        {
            CustomerType.B2B.Code.Should().Be("B2B");
        }

        #endregion

        #region FromString - Valid

        [Theory]
        [InlineData("Guest", "Guest")]
        [InlineData("Lead", "Lead")]
        [InlineData("B2C", "B2C")]
        [InlineData("B2B", "B2B")]
        public void FromString_Should_Return_Correct_CustomerType(
            string input,
            string expected)
        {
            var result = CustomerType.FromString(input);

            result.Code.Should().Be(expected);
        }

        [Theory]
        [InlineData(" Guest ")]
        [InlineData("Lead ")]
        [InlineData(" B2C")]
        [InlineData("  B2B  ")]
        public void FromString_Should_Ignore_Whitespace(string input)
        {
            var result = CustomerType.FromString(input);

            result.Should().NotBeNull();
        }

        #endregion

        #region FromString - Invalid

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Admin")]
        [InlineData("Consumer")]
        public void FromString_Should_Throw_When_Value_Is_Invalid(string value)
        {
            Action act = () => CustomerType.FromString(value);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage($"Tipo de cliente inválido: {value}");
        }

        [Fact]
        public void FromString_Should_Throw_When_Value_Is_Null()
        {
            Action act = () => CustomerType.FromString(null!);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("Tipo de cliente inválido: ");
        }

        #endregion

        #region Equality (Record)

        [Fact]
        public void CustomerType_Should_Be_Equal_When_Code_Is_Same()
        {
            var type1 = CustomerType.Guest;
            var type2 = CustomerType.FromString("Guest");

            type1.Should().Be(type2);
            type1.GetHashCode().Should().Be(type2.GetHashCode());
        }

        [Fact]
        public void CustomerType_Should_Not_Be_Equal_When_Code_Is_Different()
        {
            CustomerType.Guest.Should().NotBe(CustomerType.B2B);
        }

        #endregion

        #region ToString

        [Fact]
        public void ToString_Should_Return_Code()
        {
            CustomerType.B2C.ToString().Should().Be("B2C");
        }

        #endregion
    }
}
