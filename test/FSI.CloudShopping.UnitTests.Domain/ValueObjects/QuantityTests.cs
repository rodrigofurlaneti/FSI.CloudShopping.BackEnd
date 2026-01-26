using FSI.CloudShopping.Domain.ValueObjects;
using FSI.CloudShopping.Domain.Core;
using FluentAssertions;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class QuantityTests
    {
        [Fact]
        public void Should_Create_Quantity_When_Value_Is_Valid()
        {
            var quantity = new Quantity(1);

            quantity.Value.Should().Be(1);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Should_Throw_Exception_When_Value_Is_Less_Than_One(int value)
        {
            var act = () => new Quantity(value);

            act.Should()
                .Throw<DomainException>()
                .WithMessage("Quantity must be at least 1.");
        }

        [Fact]
        public void Implicit_Conversion_Should_Return_Int_Value()
        {
            Quantity quantity = new(5);

            int value = quantity;

            value.Should().Be(5);
        }

        [Fact]
        public void ToString_Should_Return_Value_As_String()
        {
            var quantity = new Quantity(10);

            quantity.ToString().Should().Be("10");
        }
    }
}
