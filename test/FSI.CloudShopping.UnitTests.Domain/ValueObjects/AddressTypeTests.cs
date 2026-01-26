using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class AddressTypeTests
    {
        [Fact]
        public void Shipping_Should_Have_Correct_Description()
        {
            var type = AddressType.Shipping;

            type.Description.Should().Be("Shipping");
        }

        [Fact]
        public void Billing_Should_Have_Correct_Description()
        {
            var type = AddressType.Billing;

            type.Description.Should().Be("Billing");
        }

        [Fact]
        public void Commercial_Should_Have_Correct_Description()
        {
            var type = AddressType.Commercial;

            type.Description.Should().Be("Commercial");
        }

        [Theory]
        [InlineData("Shipping")]
        [InlineData("Billing")]
        [InlineData("Commercial")]
        public void FromString_Should_Return_Correct_AddressType(string value)
        {
            var type = AddressType.FromString(value);

            type.Description.Should().Be(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Invalid")]
        [InlineData("Residential")]
        public void FromString_Should_Throw_When_Value_Is_Invalid(string value)
        {
            Action act = () => AddressType.FromString(value);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("Invalid Address Type");
        }

        [Fact]
        public void AddressType_Should_Be_Equal_When_Description_Is_Same()
        {
            var type1 = AddressType.Shipping;
            var type2 = AddressType.FromString("Shipping");

            type1.Should().Be(type2);
        }
    }
}
