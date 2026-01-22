using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;
namespace FSI.CloudShopping.UnitTests.Domain.ValueObjects
{
    public class MoneyTests
    {
        [Fact]
        public void Constructor_ShouldCreateMoney_WhenValueIsPositive()
        {
            // Arrange
            decimal expectedValue = 150.50m;
            // Act
            var money = new Money(expectedValue);
            // Assert
            money.Value.Should().Be(expectedValue);
        }
        [Fact]
        public void Constructor_ShouldThrowDomainException_WhenValueIsNegative()
        {
            // Act
            Action act = () => new Money(-10);
            // Assert
            act.Should().Throw<DomainException>()
               .WithMessage("O valor monetário não pode ser negativo.");
        }

        [Fact]
        public void Add_ShouldReturnSumOfValues()
        {
            // Arrange
            var m1 = new Money(100);
            var m2 = new Money(50);
            // Act
            var result = m1.Add(m2);
            // Assert
            result.Value.Should().Be(150);
        }

        [Fact]
        public void Multiply_ShouldReturnMultipliedValue()
        {
            // Arrange
            var price = new Money(25);
            int quantity = 3;
            // Act
            var result = price.Multiply(quantity);
            // Assert
            result.Value.Should().Be(75);
        }

        [Fact]
        public void Multiply_ShouldThrowException_WhenQuantityIsNegative()
        {
            // Arrange
            var price = new Money(25);
            // Act
            Action act = () => price.Multiply(-1);
            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Money_ShouldHaveValueEquality()
        {
            // Arrange
            var m1 = new Money(100);
            var m2 = new Money(100);
            // Assert
            m1.Should().Be(m2); 
        }
    }
}