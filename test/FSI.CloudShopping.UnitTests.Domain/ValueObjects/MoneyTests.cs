using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.Core;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class MoneyTests
    {
        #region Constructor

        [Fact]
        public void Constructor_Should_Create_Money_When_Value_Is_Valid()
        {
            // Act
            var money = new Money(100.50m);

            // Assert
            money.Value.Should().Be(100.50m);
        }

        [Fact]
        public void Constructor_Should_Allow_Zero_Value()
        {
            var money = new Money(0);

            money.Value.Should().Be(0);
        }

        [Fact]
        public void Constructor_Should_Throw_When_Value_Is_Negative()
        {
            Action act = () => new Money(-1);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("O valor monetário não pode ser negativo.");
        }

        #endregion

        #region Add

        [Fact]
        public void Add_Should_Return_Sum_Of_Two_Money_Values()
        {
            var money1 = new Money(10);
            var money2 = new Money(25);

            var result = money1.Add(money2);

            result.Value.Should().Be(35);
        }

        [Fact]
        public void Add_Should_Not_Modify_Original_Values()
        {
            var money1 = new Money(10);
            var money2 = new Money(20);

            money1.Add(money2);

            money1.Value.Should().Be(10);
            money2.Value.Should().Be(20);
        }

        #endregion

        #region Multiply

        [Fact]
        public void Multiply_Should_Return_Value_Multiplied_By_Quantity()
        {
            var money = new Money(10);

            var result = money.Multiply(3);

            result.Value.Should().Be(30);
        }

        [Fact]
        public void Multiply_Should_Allow_Zero_Quantity()
        {
            var money = new Money(10);

            var result = money.Multiply(0);

            result.Value.Should().Be(0);
        }

        [Fact]
        public void Multiply_Should_Throw_When_Quantity_Is_Negative()
        {
            var money = new Money(10);

            Action act = () => money.Multiply(-1);

            act.Should()
               .Throw<DomainException>()
               .WithMessage("A quantidade para multiplicação não pode ser negativa.");
        }

        #endregion

        #region Zero

        [Fact]
        public void Zero_Should_Return_Money_With_Zero_Value()
        {
            var zero = Money.Zero;

            zero.Value.Should().Be(0);
        }

        #endregion

        #region Equality (Record)

        [Fact]
        public void Money_Should_Be_Equal_When_Value_Is_Same()
        {
            var money1 = new Money(50);
            var money2 = new Money(50);

            money1.Should().Be(money2);
            money1.GetHashCode().Should().Be(money2.GetHashCode());
        }

        [Fact]
        public void Money_Should_Not_Be_Equal_When_Value_Is_Different()
        {
            var money1 = new Money(50);
            var money2 = new Money(75);

            money1.Should().NotBe(money2);
        }

        #endregion
    }
}
