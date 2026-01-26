using System;
using FluentAssertions;
using FSI.CloudShopping.Domain.ValueObjects;
using Xunit;

namespace FSI.CloudShopping.Domain.Tests.ValueObjects
{
    public class OrderStatusTests
    {
        #region Static Instances

        [Fact]
        public void Pending_Should_Have_Correct_Code()
        {
            OrderStatus.Pending.Code.Should().Be("Pending");
        }

        [Fact]
        public void Paid_Should_Have_Correct_Code()
        {
            OrderStatus.Paid.Code.Should().Be("Paid");
        }

        [Fact]
        public void Shipped_Should_Have_Correct_Code()
        {
            OrderStatus.Shipped.Code.Should().Be("Shipped");
        }

        [Fact]
        public void Delivered_Should_Have_Correct_Code()
        {
            OrderStatus.Delivered.Code.Should().Be("Delivered");
        }

        [Fact]
        public void Cancelled_Should_Have_Correct_Code()
        {
            OrderStatus.Cancelled.Code.Should().Be("Cancelled");
        }

        #endregion

        #region FromCode - Valid

        [Theory]
        [InlineData("Pending")]
        [InlineData("Paid")]
        [InlineData("Shipped")]
        [InlineData("Delivered")]
        [InlineData("Cancelled")]
        public void FromCode_Should_Return_Correct_OrderStatus(string code)
        {
            var result = OrderStatus.FromCode(code);

            result.Code.Should().Be(code);
        }

        [Theory]
        [InlineData(" Pending ")]
        [InlineData("Paid ")]
        [InlineData(" Shipped")]
        [InlineData("  Delivered  ")]
        [InlineData(" Cancelled ")]
        public void FromCode_Should_Ignore_Whitespace(string code)
        {
            var result = OrderStatus.FromCode(code);

            result.Should().NotBeNull();
        }

        #endregion

        #region FromCode - Invalid

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Processing")]
        [InlineData("Completed")]
        public void FromCode_Should_Throw_When_Code_Is_Invalid(string code)
        {
            Action act = () => OrderStatus.FromCode(code);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage($"Status de pedido inválido: {code}");
        }

        [Fact]
        public void FromCode_Should_Throw_When_Code_Is_Null()
        {
            Action act = () => OrderStatus.FromCode(null!);

            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("Status de pedido inválido: ");
        }

        #endregion

        #region Equality (Record)

        [Fact]
        public void OrderStatus_Should_Be_Equal_When_Code_Is_Same()
        {
            var status1 = OrderStatus.Pending;
            var status2 = OrderStatus.FromCode("Pending");

            status1.Should().Be(status2);
            status1.GetHashCode().Should().Be(status2.GetHashCode());
        }

        [Fact]
        public void OrderStatus_Should_Not_Be_Equal_When_Code_Is_Different()
        {
            OrderStatus.Pending.Should().NotBe(OrderStatus.Paid);
        }

        #endregion

        #region ToString

        [Fact]
        public void ToString_Should_Return_Code()
        {
            OrderStatus.Shipped.ToString().Should().Be("Shipped");
        }

        #endregion
    }
}
