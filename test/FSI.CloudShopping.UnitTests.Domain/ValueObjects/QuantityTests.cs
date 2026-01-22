using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class QuantityTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(9999)]
        public void Constructor_ShouldCreateQuantity_WhenValueIsPositive(int input)
        {
            // Act
            var quantity = new Quantity(input);
            // Assert
            quantity.Value.Should().Be(input);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Constructor_ShouldThrowDomainException_WhenValueIsLessThanOne(int input)
        {
            // Act
            Action act = () => new Quantity(input);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("Quantity must be at least 1.");
        }

        [Fact]
        public void Quantity_ShouldSupportValueEquality()
        {
            // Arrange
            var q1 = new Quantity(5);
            var q2 = new Quantity(5);
            // Assert
            q1.Should().Be(q2);
            (q1 == q2).Should().BeTrue();
        }

        [Fact]
        public void ImplicitOperator_ShouldConvertToIntCorrectly()
        {
            // Arrange
            var quantity = new Quantity(10);
            // Act
            int result = quantity;
            // Assert
            result.Should().Be(10);
        }
    }
}
